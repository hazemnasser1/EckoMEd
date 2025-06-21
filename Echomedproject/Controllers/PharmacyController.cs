using Echomedproject.BLL.Repositories;
using Echomedproject.DAL.Models;
using Echomedproject.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Echomedproject.BLL.Interfaces;

namespace Echomedproject.PL.Controllers
{

    public class PharmacyController : ControllerBase
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IUnitOfWork unitOfWork;
        public static string CurrentMemberEmail;
        private static readonly HttpClient client = new HttpClient();
        private const string OverpassUrl = "https://overpass-api.de/api/interpreter";



        private readonly IHttpContextAccessor _httpContextAccessor;

        public PharmacyController(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.unitOfWork = unitOfWork;
            this._httpContextAccessor = httpContextAccessor;
        }
        [Authorize("Pharmacy")]
        [HttpGet("Notifications")]
        public async Task<IActionResult> Notifications()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var userId = email.Split('@')[0];

            var user = unitOfWork.pharmacyAccRepository.getpharmacyaccWithDetails(userId);

            if (user == null)
                return NotFound("Pharmacy account not found.");

            

            if (user.Notifications == null || !user.Notifications.Any())
            {
                return Ok(new { message = "No notifications found." });
            }

            return Ok(user.Notifications);
        }


        [Authorize("Pharmacy")]
        [HttpPost("NotificationAction")]
        public async Task<IActionResult> NotificationResponse([FromBody] NotificationResponseViewModel notificationResponseViewModel)
        {
            // Check if the model passed validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the notification by ID
            var notification = unitOfWork.notificationRepository.Get(notificationResponseViewModel.notficationId);

            // Check if notification exists
            if (notification == null)
            {
                return NotFound(new { message = "Notification not found." });
            }

            // Update the state
            notification.IsExist = notificationResponseViewModel.State;

            Notification notification1 = new Notification()
            {
                UserName = notification.UserName,
                IsExist = notification.IsExist,
                PharmacyID = notification.PharmacyID,
                DateTime = DateTime.Now,
                MedicineName = notification.MedicineName,

            };

            AppUsers appUsers = unitOfWork.appUsersRepository.getUserWithRecordDetails(notification.UserName);

            appUsers.notifications ??= new List<Notification>();

            appUsers.notifications.Add(notification1);

            // Optionally, save changes if needed
            unitOfWork.Complete();

            return Ok(new { message = "Notification updated successfully." });
        }
    }
}
