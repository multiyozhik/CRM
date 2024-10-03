using CRMSystem.Authentication;
using CRMSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiAccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public ApiAccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<StatusCodeResult> LogIn([FromBody] LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
                if (user is not null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(
                        user, loginViewModel.Password, false, false);                    
                    if (result.Succeeded)
                        return new StatusCodeResult(200);
                    return new StatusCodeResult(400);
                }
                return new StatusCodeResult(401);
            }
            return new StatusCodeResult(400);             
        }

        [Authorize]
        [HttpGet, ValidateAntiForgeryToken]
        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
