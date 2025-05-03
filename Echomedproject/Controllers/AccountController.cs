using System.Text.Encodings.Web;
using System.Text;
using Echomedproject.DAL.Models;
using Echomedproject.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Echomedproject.BLL.Repositories;
using Echomedproject.BLL.Interfaces;
using Echomedproject.PL.Helpers;

namespace Echomedproject.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> userManager;
        private IUnitOfWork unitOfWork;
        private readonly SignInManager<Users> signInManager;
        private readonly IEmailSender emailSender;


        public AccountController(UserManager<Users> usermanager,SignInManager<Users> signInManager,IEmailSender emailSender,IUnitOfWork unitOfWork)
        
        { 
            this.userManager = usermanager;
            this.unitOfWork = unitOfWork;

            this.signInManager = signInManager;
            this.emailSender = emailSender;

        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {


            if (ModelState.IsValid)
            {

                var user = new Users()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    DateOBirth = model.DateOfBirth,
                    UserName = model.Username,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    city = model.City,
                    street = model.Street,
                    country = model.Country,
                    imagePath = model.ProfilePicture.FileName,

                };
                
               
                var Result=await userManager.CreateAsync(user,model.Password);
                

                if (Result.Succeeded)
                {
                    var appuser = new AppUsers()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Gender = model.Gender,
                        DateOfBirth = model.DateOfBirth,
                        UserName = model.Username,
                        PhoneNum = model.PhoneNumber,
                        Email = model.Email,
                        City = model.City,
                        street = model.Street,
                        Country = model.Country,

                    };
                    unitOfWork.appUsersRepository.add(appuser);
                    string ImagePath = DocumentSetting.UploadImage(model.ProfilePicture, "Images");
                    var userUpdate= await userManager.FindByEmailAsync(user.Email);
                    userUpdate.imagePath=ImagePath;
                    var resultimg = await userManager.UpdateAsync(user);
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = tokenEncoded }, Request.Scheme);

                    var message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.";

                    await emailSender.SendEmailAsync(user.Email, "Confirm Your Email", message);
                    unitOfWork.Complete();
                    return RedirectToAction("EmailVerificationNotice");

                }

                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


            }


            return View(model);
        }

        public IActionResult EmailVerificationNotice()
        {
            return View(); // tell the user to check their inbox
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Unable to load user with ID '{userId}'.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return View("ConfirmEmailSuccess");

            return View("Error");
        }

        public IActionResult ConfirmEmailSuccess()
        {
            return View();
        }



        public IActionResult Login()
        {
            return View();
        }

    }


}
