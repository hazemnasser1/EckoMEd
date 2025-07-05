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
using System.Security.Claims;

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

        [HttpPost("user-register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Generate date prefix (yyMMdd)
            var datePrefix = DateTime.Today.ToString("yyMMdd"); // e.g., "250925"

            // Fetch latest user with matching date prefix
            var lastUser = unitOfWork.appUsersRepository
                .GetAll()
                .Where(u => u.UserName.StartsWith(datePrefix))
                .OrderByDescending(u => u.UserName)
                .FirstOrDefault();

            int nextNumber = 1;
            if (lastUser != null && lastUser.UserName.Length >= 11)
            {
                var lastNumberPart = lastUser.UserName.Substring(6); // "00001"
                if (int.TryParse(lastNumberPart, out int parsed))
                    nextNumber = parsed + 1;
            }

            var generatedUserName = $"{datePrefix}{nextNumber.ToString("D5")}"; // e.g., 25092500001

            // Create Identity User
            var user = new Users
            {
                FirstName = model.firstName,
                LastName = model.lastName,
                Gender = model.gender,
                DateOBirth = model.dateOfBirth,
                UserName = generatedUserName,
                PhoneNumber = model.phoneNumber,
                Email = model.email,
                city = model.city,
                street = model.street,
                country = model.country,
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
                UserName = generatedUserName,
                PhoneNum = model.phoneNumber,
                Email = model.email,
                City = model.city,
                street = model.street,
                Country = model.country,
                CardNumber = "#",
                CVC = 0,
                ExpirationDate = "#"
            };

            unitOfWork.appUsersRepository.add(appuser);

            // Upload profile image
            if (model.profilePicture != null)
            {
                string imagePath = DocumentSetting.UploadImage(model.profilePicture, "userImages");
                user.imagePath = imagePath;
            }
            else
            {
                user.imagePath = "default.jpg";
            }
            await userManager.UpdateAsync(user);


            await userManager.AddToRoleAsync(user, "User");

            // Email confirmation
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = $"{Request.Scheme}://{Request.Host}/api/account/confirm-email?userId={user.Id}&token={tokenEncoded}";

            var message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.";
            await emailSender.SendEmailAsync(user.Email, "Confirm Your Email", message);

            unitOfWork.Complete();

            return Ok(new
            {
                success = true, message = "Registration successful. Please check your email to confirm your account.",
            });
        }


        [HttpPost("DataEntry-register")]
        public async Task<IActionResult> Data_Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new Users
            {
                FirstName = model.firstName,
                LastName = model.lastName,
                Gender = model.gender,
                DateOBirth = model.dateOfBirth,
                UserName = "Nile_hospital",
                PhoneNumber = model.phoneNumber,
                Email = model.email,
                city = model.city,
                street = model.street,
                country = model.country,
                EmailConfirmed = true // <--- this line confirms the email automatically
            };


            var result = await userManager.CreateAsync(user, model.password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Add to AppUsers table
            var appuser = new DataEntry
            {
                Username = model.firstName + " " + model.lastName,
                EntryDate = model.dateOfBirth,
                PhoneNumber = model.phoneNumber,
                Email = model.email,
                HospitalID = 144,
                Hospital = unitOfWork.hospitalsRepository.Get(144),
                TotalPatients = 0,
                Departments = new List<Departments>()

            };
            unitOfWork.entryRepository.add(appuser);

            //Upload image
            if (model.profilePicture != null)
            {
                string imagePath = DocumentSetting.UploadImage(model.profilePicture, "userImages");
                user.imagePath = imagePath;
            }
            await userManager.UpdateAsync(user);
            await userManager.AddToRoleAsync(user, "DataEntry");

            // Email confirmation
            //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            //var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            //var confirmationLink = $"{Request.Scheme}://{Request.Host}/api/account/confirm-email?userId={user.Id}&token={tokenEncoded}";

            //var message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.";
            //await emailSender.SendEmailAsync(user.Email, "Confirm Your Email", message);

            unitOfWork.Complete();

            return Ok(new {success=true , message = "Registration successful. Please check your email to confirm your account." });
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

        private async Task<IActionResult> RoleLogin(LoginViewModel model, string expectedRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(model.email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            if (!await userManager.IsEmailConfirmedAsync(user))
                return Unauthorized("Please confirm your email before logging in.");

            if (await userManager.IsLockedOutAsync(user))
                return Unauthorized("Your account is locked. Please try again later or contact support.");

            var passwordValid = await userManager.CheckPasswordAsync(user, model.password);
            if (!passwordValid)
                return Unauthorized("Incorrect password.");

            if (!await userManager.IsInRoleAsync(user, expectedRole))
                return Unauthorized($"Access denied. You are not a {expectedRole}.");

            var result = await signInManager.PasswordSignInAsync(user, model.password, isPersistent: model.rememberMe, lockoutOnFailure: true);
            if (!result.Succeeded)
                return Unauthorized("Invalid login attempt.");

            return Ok(new { message = $"{expectedRole} login successful." });
        }

        [HttpPost("userlogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginViewModel model)
        {
            return await RoleLogin(model, "User");
        }

        [HttpPost("dataentrylogin")]
        public async Task<IActionResult> DataEntryLogin([FromBody] LoginViewModel model)
           {
            return await RoleLogin(model, "DataEntry");
        }

        [HttpPost("pharmacylogin")]
        public async Task<IActionResult> PharmacyLogin([FromBody] LoginViewModel model)
        {
            return await RoleLogin(model, "Pharmacy");
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
        [HttpGet("is-authenticated")]
        public IActionResult IsAuthenticated()
        {
            var isAuthenticated = User?.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
                return Ok(new { isAuthenticated = false });

            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var role = roleClaim?.Value ?? "Unknown";

            return Ok(new
            {
                isAuthenticated = true,
                role = role
            });
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                return Ok(new { message = "If the email exists, a password reset link has been sent." }); // Avoid exposing user existence

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var resetLink = $"http://localhost:4200/forgotpas?email={user.Email}&token={encodedToken}";

            var message = $"You can reset your password by <a href='{HtmlEncoder.Default.Encode(resetLink)}'>clicking here</a>.";
            await emailSender.SendEmailAsync(user.Email, "Reset Your Password", message);

            return Ok(new { message = "If the email exists, a password reset link has been sent." });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { message = "Invalid request." });

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            var result = await userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
            if (result.Succeeded)
                return Ok(new { message = "Password reset successful." });

            return BadRequest(result.Errors);
        }
        [HttpGet("access-denied")]
        public async Task<IActionResult> access_denied()
        {
            return StatusCode(403, new
            {
                error = "Access Denied",
                message = "You do not have permission to access this resource."
            });
        }


    }
}
