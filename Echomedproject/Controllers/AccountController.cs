using System.Text.Encodings.Web;
using System.Text;
using Echomedproject.DAL.Models;
using Echomedproject.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Echomedproject.BLL.Repositories;
using Echomedproject.BLL.Interfaces;
using Echomedproject.PL.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Echomedproject.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IUnitOfWork unitOfWork;

        public AccountController(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new Users
            {
                FirstName = model.firstName,
                LastName = model.lastName,
                Gender = model.gender,
                DateOBirth = model.dateOfBirth,
                UserName = model.username,
                PhoneNumber = model.phoneNumber,
                Email = model.email,
                city = model.city,
                street = model.street,
                country = model.country,
                imagePath = model.profilePic
            };

            var result = await userManager.CreateAsync(user, model.password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Add to AppUsers table
            var appuser = new AppUsers
            {
                FirstName = model.firstName,
                LastName = model.lastName,
                Gender = model.gender,
                DateOfBirth = model.dateOfBirth,
                UserName = model.username,
                PhoneNum = model.phoneNumber,
                Email = model.email,
                City = model.city,
                street = model.street,
                Country = model.country
            };
            unitOfWork.appUsersRepository.add(appuser);

            // Upload image
            //string imagePath = DocumentSetting.UploadImage(model.ProfilePicture, "Images");
            //user.imagePath = imagePath;
            //await userManager.UpdateAsync(user);

            // Email confirmation
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = tokenEncoded }, Request.Scheme);

            var message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.";
            await emailSender.SendEmailAsync(user.Email, "Confirm Your Email", message);

            unitOfWork.Complete();

            return Ok(new { message = "Registration successful. Please check your email to confirm your account." });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Missing parameters.");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"User with ID '{userId}' not found.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return Ok("Email confirmed successfully.");

            return BadRequest("Email confirmation failed.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            // Check if email is confirmed
            if (!await userManager.IsEmailConfirmedAsync(user))
                return Unauthorized("Please confirm your email before logging in.");

            // Check if account is locked out
            if (await userManager.IsLockedOutAsync(user))
                return Unauthorized("Your account is locked. Please try again later or contact support.");

            // Optional: Check password manually before signing in
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                // You could increment failed attempts here manually if needed
                return Unauthorized("Incorrect password.");
            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);
            if (!result.Succeeded)
                return Unauthorized("Invalid login attempt.");

            return Ok(new { message = "Login successful." });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user (removes authentication cookie)
            await signInManager.SignOutAsync();

            // Optional: Clear any additional cookies (if you manually set any)
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return Ok(new { message = "Logout successful. All cookies cleared." });
        }


    }
}
