using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            })
                    .AddRoles<AppRole>()
                    .AddRoleManager<RoleManager<AppRole>>()
                    .AddSignInManager<SignInManager<AppUser>>()
                    .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

                    // JWT configuration
                    .AddJwtBearer(options =>
                    {
                           options.TokenValidationParameters = new TokenValidationParameters
                           {
                               ValidateIssuerSigningKey = true,
                               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                               ValidateIssuer = false,
                               ValidateAudience = false
                           };
                        })

                    // cookie configuration
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                           options.Cookie.HttpOnly = true;
                           options.Cookie.SameSite = SameSiteMode.None;
                           options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

                    });

            return services;
        }
    }
}
