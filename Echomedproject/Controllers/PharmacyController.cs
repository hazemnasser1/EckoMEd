using Echomedproject.BLL.Repositories;
using Echomedproject.DAL.Models;
using Echomedproject.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Migrations;

namespace Echomedproject.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [Authorize(Roles = "Pharmacy")]
        [HttpGet("Pending-requests")]
        public async Task<IActionResult> Pending_requests()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var userId = email.Split('@')[0];
            var user = unitOfWork.pharmacyAccRepository.getpharmacyaccWithDetails(userId);

            if (user == null)
                return NotFound("Pharmacy account not found.");

            var allRequests = user.Requests;

            if (allRequests == null || !allRequests.Any())
                return Ok(new { message = "No requests found." });

            // Stats
            var totalRequests = allRequests.Count;
            var pendingRequests = allRequests.Count(r => string.IsNullOrEmpty(r.state) || r.state.ToLower() == "pending");
            var closedTodayRequests = allRequests.Count(r =>
                r.state?.ToLower() == "closed" &&
                r.ClosedAt?.Date == DateTime.Today);

            // Pending items
            var pendingList = allRequests
                .Where(r => string.IsNullOrEmpty(r.state) || r.state.ToLower() == "pending")
                .Select(r => new RequestsdisplayViewModel
                {
                    Id = r.Id,
                    MedicineName = r.MedicineName,
                    qty = r.qty,
                    state = r.state ?? "pending",
                    UserName = r.AppUser?.UserName,
                    Email = r.AppUser?.Email,
                    phoneNum = r.AppUser?.PhoneNum
                }).ToList();

            return Ok(new
            {
                totalRequests,
                closedTodayRequests,
                pendingRequests,
                pendingItems = pendingList
            });
        }


        [Authorize(Roles = "Pharmacy")]
        [HttpGet("Closed-requests")]
        public async Task<IActionResult> Closed_requests()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var userId = email.Split('@')[0];
            var user = unitOfWork.pharmacyAccRepository.getpharmacyaccWithDetails(userId);

            if (user == null)
                return NotFound("Pharmacy account not found.");

            var closedRequests = user.Requests?
                .Where(r => r.state?.ToLower() == "closed")
                .ToList();

            if (closedRequests == null || !closedRequests.Any())
            {
                return Ok(new { message = "No closed requests found." });
            }

            var approvedCount = closedRequests.Count(r => r.Response?.ToLower() == "approved");
            var rejectedCount = closedRequests.Count(r => r.Response?.ToLower() == "rejected");
            var totalClosed = closedRequests.Count;

            var closedList = closedRequests
                .Select(r => new RequestsdisplayViewModel
                {
                    Id = r.Id,  // ← Include ID
                    MedicineName = r.MedicineName,
                    qty = r.qty,
                    state = r.Response ?? "unknown",  // Show "approved"/"rejected"
                    UserName = r.AppUser?.UserName,
                    Email = r.AppUser?.Email,
                    phoneNum = r.AppUser?.PhoneNum
                })
                .ToList();

            return Ok(new
            {
                totalClosed,
                approvedCount,
                rejectedCount,
                closedItems = closedList
            });
        }

        [Authorize(Roles = "Pharmacy")]
        [HttpPost("Approve-request")]
        public async Task<IActionResult> ApproveRequest([FromBody]int id)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var userId = email.Split('@')[0];
            var user = unitOfWork.pharmacyAccRepository.getpharmacyaccWithDetails(userId);
            if (user == null)
                return NotFound("Pharmacy account not found.");

            var request = user.Requests?.FirstOrDefault(r => r.Id == id);
            if (request == null)
                return NotFound("Request not found or doesn't belong to this pharmacy.");

            if (request.state?.ToLower() == "closed")
                return BadRequest("This request is already closed.");

            request.state = "closed";
            request.Response = "approved";
            request.ClosedAt = DateTime.Now;

            var appUser = request.AppUser;
            if (appUser != null)
            {
                appUser.notifications ??= new List<Notification>();
                appUser.notifications.Add(new Notification
                {
                    Text = $"Your request for '{request.MedicineName}' has been approved.",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    Type = "RequestStatus",
                    PharmacyName = user.Pharmacy.Name

                });
            }

            unitOfWork.Complete();
            return Ok(new { message = "Request approved successfully." });
        }



        [Authorize(Roles = "Pharmacy")]
        [HttpPost("Reject-request")]
        public async Task<IActionResult> RejectRequest([FromBody] int id)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var userId = email.Split('@')[0];
            var user = unitOfWork.pharmacyAccRepository.getpharmacyaccWithDetails(userId);
            if (user == null)
                return NotFound("Pharmacy account not found.");

            var request = user.Requests?.FirstOrDefault(r => r.Id == id);
            if (request == null)
                return NotFound("Request not found or doesn't belong to this pharmacy.");

            if (request.state?.ToLower() == "closed")
                return BadRequest("This request is already closed.");

            request.state = "closed";
            request.Response = "rejected";
            request.ClosedAt = DateTime.Now;

            var appUser = request.AppUser;
            if (appUser != null)
            {
                appUser.notifications ??= new List<Notification>();
                appUser.notifications.Add(new Notification
                {
                    Text = $"Your request for '{request.MedicineName}' has been rejected.",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    Type = "RequestStatus",
                    PharmacyName = user.Pharmacy.Name
                });
            }

            unitOfWork.Complete();
            return Ok(new { message = "Request rejected successfully." });
        }


        [Authorize(Roles = "Pharmacy")]
        [HttpPost("Toggle-response")]
        public async Task<IActionResult> ToggleResponse([FromBody] int id)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var userId = email.Split('@')[0];
            var user = unitOfWork.pharmacyAccRepository.getpharmacyaccWithDetails(userId);
            if (user == null)
                return NotFound("Pharmacy account not found.");

            var request = user.Requests?.FirstOrDefault(r => r.Id == id);
            if (request == null)
                return NotFound("Request not found or doesn't belong to this pharmacy.");

            if (request.state?.ToLower() != "closed" || string.IsNullOrEmpty(request.Response))
                return BadRequest("Request must already be closed with a response to toggle.");

            // Toggle
            string newResponse = request.Response.ToLower() == "approved" ? "rejected" : "approved";
            request.Response = newResponse;
            request.ClosedAt = DateTime.Now;

            var appUser = request.AppUser;
            if (appUser != null)
            {
                appUser.notifications ??= new List<Notification>();
                appUser.notifications.Add(new Notification
                {
                    Text = $"Your request for '{request.MedicineName}' has been changed to {newResponse}.",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    Type = "RequestStatus",
                    PharmacyName = user.Pharmacy.Name
                });
            }

            unitOfWork.Complete();
            return Ok(new { message = $"Request response toggled to '{newResponse}'." });
        }


    }
}
