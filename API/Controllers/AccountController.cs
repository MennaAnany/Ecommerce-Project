using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
     //   private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,/* ITokenService tokenService*/ IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        //    _tokenService = tokenService;
            _mapper = mapper;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

           // var token = await _tokenService.CreateToken(user);

            // Sign in with cookie authentication
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }, CookieAuthenticationDefaults.AuthenticationScheme)));

            return new UserDto
            {
                Id = user.Id,
             //   Token = token,
                Username = user.UserName,
                Email = user.Email
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized("Incorrect Email!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return BadRequest("Incorrect email or password");

          //  var token = await _tokenService.CreateToken(user);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }, CookieAuthenticationDefaults.AuthenticationScheme)));

            return new UserDto
            {
                Id = user.Id,
              //  Token = token,
                Username = user.UserName,
                Email = user.Email
            };
        }

        [Authorize]
        [HttpGet("get-me")]
        public async Task<ActionResult<UserDto>> GetMe()
        {
            var userId = User.GetUserId();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return Unauthorized("Email Not Found");

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            };
        }

        [Authorize]
        [HttpDelete("delete-me")]
        public async Task<ActionResult> DeleteMe()
        {
            var userId = User.GetUserId();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return NoContent();

        }


        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            // Clear the existing cookie
            await HttpContext.SignOutAsync(
             CookieAuthenticationDefaults.AuthenticationScheme);

            return NoContent();
        }
    }
}

