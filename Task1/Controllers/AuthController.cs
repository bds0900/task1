using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task1.Models;

namespace Task1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        [ActionName("Authorized")]
        public IActionResult Authorized()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login )
        {

            var user = await _userManager.FindByNameAsync(login.Username);
            if (user != null)
            {
                var signinResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                if (signinResult.Succeeded)
                {
                    //return Ok("login success");
                    return Ok(new LoginResult{Succeeded=true,Message= "login success" });
                }
                else
                {
                    //return Ok("incorrect password");
                    return BadRequest(new LoginResult { Succeeded = false, Message = "invalid password" });
                }
            }

            return BadRequest(new LoginResult { Succeeded = false, Message = "Username is not founded" });
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(SignInModel signin)
        {
            //RegisterViewModel에서 [require][compare]같은 속성을 사용했기 때문에 여기서 ModelState.IsValid사용
            //model is invalid reutrn 
            if (!ModelState.IsValid)
            {
                return BadRequest(new SigninResult { Succeeded = false, Message = "invalid input" });
            }
            var user = new AppUser(signin.Username);
            var result = await _userManager.CreateAsync(user, signin.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok(new SigninResult { Succeeded = true, Message = "Successfully create account" });
            }

            return BadRequest(new SigninResult { Succeeded = false, Message = result.Errors.FirstOrDefault().Description });
        }
    }
}
