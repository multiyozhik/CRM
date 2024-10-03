using CRMSystem.Authentication;
using CRMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace CRMSystem.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiAdminController : ControllerBase
    {
        readonly UserManager<AppUser> userManager;
        readonly IPasswordHasher<AppUser> passwordHasher;
        readonly IConfiguration config;

        public ApiAdminController(
            UserManager<AppUser> userManager,
            IPasswordHasher<AppUser> passwordHasher,
            IConfiguration config)
        {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.config = config;
        }

        [HttpPost]
        public async Task<StatusCodeResult> CreateAppUser([FromBody] AppUserDataFromRequest appUserData)
        {
            if (ModelState.IsValid)
            {
                AppUser newAppUser = new()
                {
                    UserName = appUserData.UserName,
                    Email = appUserData.Email
                };
                IdentityResult result = await userManager.CreateAsync(newAppUser, appUserData.Password);
                string? adminRole = config["AdminAccountData:Role"];
                if (result.Succeeded && adminRole is not null && !adminRole.IsNullOrEmpty())
                {
                    await userManager.AddToRoleAsync(newAppUser, adminRole);
                    return new StatusCodeResult(200);
                }
            }
            return new StatusCodeResult(400);
        }

        [HttpPost("{id}")]
        public async Task<StatusCodeResult> EditAppUser(
            [FromRoute] string id, [FromBody] AppUserDataFromRequest appUserData)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await userManager.FindByIdAsync(id);
                if (user is not null)
                {
                    user.UserName = appUserData.UserName;
                    user.Email = appUserData.Email;
                    user.PasswordHash = passwordHasher.HashPassword(user, appUserData.Password);
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return new StatusCodeResult(200);                    
                }
                return new StatusCodeResult(404);
            }
            return new StatusCodeResult(400);
        }

        [HttpPost("{id}")]  
        public async Task<StatusCodeResult> DeleteAppUser([FromRoute] string id)
        {
            AppUser? user = await userManager.FindByIdAsync(id);
            if (user is not null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return new StatusCodeResult(200);
                return new StatusCodeResult(404);
            }
            return new StatusCodeResult(401);
        }
    }
    public record AppUserDataFromRequest(string UserName, string Password, string Email);
}
