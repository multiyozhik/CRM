using Microsoft.EntityFrameworkCore;
using CRMSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Authentication
{
    //если наследовать просто от DbContext, то ошибка, что "Cannot create a DbSet for 'IdentityUserClaim<string>'
    //because this type is not included in the model for the context"
    //https://github.com/dotnet/aspnetcore/issues/5793

    public class AppUsersDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser>? AppUsers { get; }
        public AppUsersDbContext(DbContextOptions<AppUsersDbContext> options) : base(options) 
        {
            base.Database.EnsureCreated();            
        }        
    }
}
