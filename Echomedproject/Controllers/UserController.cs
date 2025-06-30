using System;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Models;
using Echomedproject.PL.Helpers;
using Echomedproject.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Echomedproject.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IUnitOfWork unitOfWork;
        public static string CurrentMemberEmail;
        private static readonly HttpClient client = new HttpClient();
        private const string OverpassUrl = "https://overpass-api.de/api/interpreter";



        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(
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

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> ShowProfile()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;


            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var appUser = await userManager.GetUserAsync(currentUser);

            if (appUser == null)
                return NotFound("User not found.");

            AppUsers user = unitOfWork.appUsersRepository.getUserbyEmail(email);

            if (user == null)
                return NotFound("User not found.");


            return Ok(new
            {
                firstName = appUser.FirstName,
                lastName = appUser.LastName,
                image = DocumentSetting.GetBase64Image(appUser.imagePath,"userImage"),
                insurance = user.Insurance
            });
        }

        [Authorize]
        [HttpPost("UpdateProfileImage")]
        public async Task<IActionResult> UpdateProfileImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Image file is required.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var appUser = await userManager.FindByEmailAsync(email);
            if (appUser == null)
                return NotFound("User not found.");

            // Upload image
            var fileName = DocumentSetting.UploadImage(file, "userImage");

            // Update user's image path
            appUser.imagePath = fileName;

            var result = await userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
                return BadRequest("Failed to update profile image.");

            return Ok(new
            {
                message = "Profile image updated successfully.",
                base64Image = DocumentSetting.GetBase64Image(fileName, "userImage")
            });
        }


        [Authorize]
        [HttpPost("Profile")]
        public async Task<IActionResult> EditProfile([FromBody] EditViewModel editViewModel)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;

            if (currentUser == null)
                return Unauthorized();

            var appUser = await userManager.GetUserAsync(currentUser);

            if (appUser == null)
                return NotFound("User not found.");

            appUser.FirstName = editViewModel.Firstname;
            appUser.LastName = editViewModel.Lastname;

            var result = await userManager.UpdateAsync(appUser);

            if (result.Succeeded)
                return Ok("Profile updated successfully.");

            return BadRequest("Failed to update profile.");
        }


        [Authorize]
        [HttpPost("CardData")]
        public async Task<IActionResult> CardDetails([FromBody] CardViewModel cardviewmodel)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            AppUsers user = unitOfWork.appUsersRepository.getUserbyEmail(email);

            if (user == null)
                return NotFound("User not found.");

            user.CardNumber = cardviewmodel.CardNumber;
            user.CVC = cardviewmodel.CVC;
            user.ExpirationDate = cardviewmodel.ExpirationDate;

            unitOfWork.Complete();

            return Ok("Card details updated successfully.");
        }

        [HttpPost("GetInsurance")]
        [Authorize]

        public async Task<IActionResult> InsuranceDetails([FromBody] string Insurance)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            AppUsers user = unitOfWork.appUsersRepository.getUserbyEmail(email);
            if (user == null)
                return NotFound("User not found.");
            user.Insurance = Insurance;
            //unitOfWork.appUsersRepository.update(user);
            unitOfWork.Complete();
            return Ok("Insurance updated successfully.");
        }

        [HttpPost("RemoveInsurance")]
        [Authorize]
        public async Task<IActionResult> RemoveInsurance()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            AppUsers user = unitOfWork.appUsersRepository.getUserbyEmail(email);
            if (user == null)
                return NotFound("User not found.");

            user.Insurance = null; // or use: string.Empty;
            unitOfWork.Complete();

            return Ok("Insurance removed successfully.");
        }

        //[HttpGet("Get-Hospitals")]
        //public async Task<IActionResult> Hospital_filll()
        //{
        //    string query = @"
        //[out:json][timeout:30];
        //(
        //  node[""amenity""=""Hospitals""](29.9,30.9,30.1,31.3);
        //  way[""amenity""=""Hospitals""](29.9,30.9,30.1,31.3);
        //  relation[""amenity""=""Hospitals""](29.9,30.9,30.1,31.3);
        //);
        //out center;
        //";


        //    var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");
        //    using var client = new HttpClient();
        //    var response = await client.PostAsync("https://overpass-api.de/api/interpreter", content);

        //    if (!response.IsSuccessStatusCode)
        //        return StatusCode((int)response.StatusCode, "Failed to fetch data from Overpass API");

        //    var jsonString = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<OverpassResponse>(jsonString);

        //    var insuranceOptions = new List<string>
        //    {
        //        "CairoCare", "MisrHealth", "NileSecure", "Takaful Egypt", "Al Hayah", "GlobalMed Cairo",
        //        "HealthNet Egypt", "LifeSecure", "Delta Insurance", "SahhaCare", "SmartHealth", "ShifaPlan"
        //    };

        //    var departmentNames = new List<string>
        //    {
        //        "Cardiology", "Neurology", "Orthopedics", "Pediatrics", "Oncology", "Dermatology", "Radiology",
        //        "Gastroenterology", "Nephrology", "Endocrinology", "Emergency", "ICU", "ENT", "Psychiatry", "Urology"
        //    };

        //    var hospitals = result.Elements
        //        .Where(e => e.Tags != null && e.Tags.ContainsKey("name"))
        //        .Select(e => new
        //        {
        //            Name = e.Tags["name"],
        //            Latitude = e.Lat ?? e.Center?.Lat,
        //            Longitude = e.Lon ?? e.Center?.Lon
        //        })
        //        .Where(h => h.Latitude != null && h.Longitude != null)
        //        .ToList();

        //    foreach (var hospitalData in hospitals)
        //    {
        //        var existingHospital = await unitOfWork.hospitalsRepository
        //            .FindAsync(h => h.Name.ToLower() == hospitalData.Name.ToLower());

        //        if (existingHospital != null)
        //            continue; // Skip if hospital already exists

        //        var rand = new Random();

        //        var selectedDepartments = departmentNames
        //            .OrderBy(x => rand.Next())
        //            .Take(10)
        //            .Select(name => new Departments
        //            {
        //                Name = name,
        //                Capacity = 0,
        //                Description = $"{name} department",
        //                TotalPatients = 0,
        //                budget = rand.Next(200, 1000),
        //                Rooms = null
        //            })
        //            .ToList();

        //        var selectedInsurances = insuranceOptions
        //            .OrderBy(x => rand.Next())
        //            .Take(4)
        //            .Select(name => new Insurance { Name = name })
        //            .ToList();

        //        var hospital = new Hospitals
        //        {
        //            Name = hospitalData.Name,
        //            Address = "Unknown Address",
        //            City = "Giza",
        //            State = "Giza",
        //            Country = "Egypt",
        //            ZipCode = "00000",
        //            PhoneNumber = "0123456789",
        //            Email = "info@hospital.com",
        //            Website = "http://hospital.com",
        //            TotalRooms = 0,
        //            TotalBeds = 0,
        //            ICUBeds = 0,
        //            EmergencyRooms = 0,
        //            Latitude = hospitalData.Latitude,
        //            Longitude = hospitalData.Longitude,
        //            AcceptedInsurances = selectedInsurances,
        //            Departments = selectedDepartments,
        //            DateEntryId = 1
        //        };

        //        foreach (var dep in selectedDepartments)
        //        {
        //            dep.Hospital = hospital;
        //        }

        //        unitOfWork.hospitalsRepository.add(hospital);
        //    }

        //    unitOfWork.Complete();

        //    return Ok("Hospitals added successfully");
        //}

        //      [HttpGet("Get-Pharmacy")]
        //      public async Task<IActionResult> Hospital_filll()
        //      {
        //          string query = @"
        //      [out:json][timeout:30];
        //      (
        //        node[""amenity""=""pharmacy""](30.0,31.1,30.2,31.5);
        //way[""amenity""=""pharmacy""](30.0,31.1,30.2,31.5);
        //relation[""amenity""=""pharmacy""](30.0,31.1,30.2,31.5);
        //      );
        //      out center;
        //      ";
        //          var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");
        //          using var client = new HttpClient();
        //          var response = await client.PostAsync("https://overpass-api.de/api/interpreter", content);

        //          if (!response.IsSuccessStatusCode)
        //              return StatusCode((int)response.StatusCode, "Failed to fetch data from Overpass API");

        //          var jsonString = await response.Content.ReadAsStringAsync();
        //          var result = JsonConvert.DeserializeObject<OverpassResponse>(jsonString);
        //          int baseId = 20250113;

        //          foreach (var element in result.Elements)
        //          {
        //              string identifier = baseId.ToString();

        //              var name = element.Tags != null && element.Tags.TryGetValue("name", out var tagName)
        //                  ? tagName
        //                  : $"Pharmacy-{Guid.NewGuid().ToString().Substring(0, 6)}"; // fallback name if needed (not used in identity anymore)

        //              if (string.IsNullOrWhiteSpace(name))
        //                  continue;

        //              // Check if pharmacy with this name already exists
        //              var existingPharmacy = await unitOfWork.pharamacyRepository
        //                  .FindAsync(p => p.Name.ToLower() == name.ToLower());

        //              if (existingPharmacy != null)
        //                  continue;

        //              var pharmacy = new Pharmacy
        //              {
        //                  Identifier = identifier,
        //                  Name = name,
        //                  City = "Giza",
        //                  State = "Giza",
        //                  Country = "Egypt",
        //                  Latitude = element.Lat ?? element.Center?.Lat,
        //                  Longitude = element.Lon ?? element.Center?.Lon
        //              };

        //              unitOfWork.pharamacyRepository.add(pharmacy);

        //              // Create Identity user using the identifier
        //              var user = new Users
        //              {
        //                  UserName = identifier,
        //                  Email = $"{identifier}@eckomed.com",
        //                  EmailConfirmed = true
        //              };

        //              var resultUser = await userManager.CreateAsync(user, "Default@123");
        //              await userManager.AddToRoleAsync(user, "Pharmacy");

        //              if (resultUser.Succeeded)
        //              {
        //                  var pharmacyAcc = new PharmacyAcc
        //                  {
        //                      Username = user.UserName,
        //                      Email = user.Email,
        //                      PhoneNumber = user.UserName,
        //                      EntryDate = DateTime.UtcNow,
        //                      Pharmacy = pharmacy
        //                  };

        //                  unitOfWork.pharmacyAccRepository.add(pharmacyAcc);
        //              }
        //              else
        //              {
        //                  var errors = string.Join(", ", resultUser.Errors.Select(e => e.Description));
        //                  Console.WriteLine($"Failed to create user for pharmacy ID {identifier}: {errors}");
        //              }


        //              baseId++; // increment for next pharmacy
        //          }




        //          unitOfWork.Complete();

        //          return Ok("Hospitals added successfully");
        //      }
        [Authorize]
        [HttpGet("hospitalsearch")]
        public async Task<IActionResult> HospitalSearch([FromQuery] HospitalSearchViewModel hospitalSearchViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            hospitalSearchViewModel.Lat ??= 0;
            hospitalSearchViewModel.Lang ??= 0;

            var userLat = hospitalSearchViewModel.Lat.Value;
            var userLng = hospitalSearchViewModel.Lang.Value;

            string depType = hospitalSearchViewModel.Deptype?.ToLower();
            string insurance = hospitalSearchViewModel.Insurance?.ToLower();
            string name = hospitalSearchViewModel.Name?.ToLower();
            double? budget = hospitalSearchViewModel.Budget;
            double? maxDistance = hospitalSearchViewModel.Distance ??= 9999;

            var hospitals = await unitOfWork.hospitalsRepository.GetAllHospitalsWithDetailsAsync();

            var results = new List<SearchResultViewModel>();

            foreach (var hospital in hospitals)
            {
                if (!hospital.Departments.Any(d => d.Name.ToLower().Contains(depType)))
                    continue;

                if (!string.IsNullOrEmpty(name) && !hospital.Name.ToLower().Contains(name))
                    continue;

                if (!string.IsNullOrEmpty(insurance) &&
                    !hospital.AcceptedInsurances.Any(i => i.Name.ToLower().Contains(insurance)))
                    continue;

                var matchingDept = hospital.Departments.FirstOrDefault(d => d.Name.ToLower().Contains(depType));
                if (budget.HasValue && (matchingDept?.budget ?? 0) > budget.Value)
                    continue;

                double distance = 0;
                if (userLat > 0 && userLng > 0)
                {
                    if (!hospital.Latitude.HasValue || !hospital.Longitude.HasValue)
                        continue;

                    distance = DistanceCalc.CalculateHaversineDistance(userLat, userLng, hospital.Latitude.Value, hospital.Longitude.Value);

                    if (distance > maxDistance.Value)
                        continue;
                }
                int WaitingPatients = matchingDept.TotalPatients - matchingDept.NomOfDoctors;
                int lowerTimeRange = WaitingPatients * 20;
                int upperTimeRange= WaitingPatients * 30;
                results.Add(new SearchResultViewModel
                {
                    HospitalName = hospital.Name,
                    Distance = distance,
                    TotalPatients=matchingDept.TotalPatients,
                    lowerRange = lowerTimeRange,
                    upperRange=upperTimeRange,
                    Budget = matchingDept?.budget ?? 0,
                    Department = matchingDept?.Name,
                    Insurance = !string.IsNullOrEmpty(insurance) &&
                                hospital.AcceptedInsurances.Any(i => i.Name.ToLower().Contains(insurance))
                });
            }

            return Ok(results);
        }


        [Authorize]
        [HttpGet("Get-Ads")]
        public async Task<IActionResult> GetAds()
        {

            var advertisements = unitOfWork.adverismentRepository.GetAll().ToList();

            var adsWithImages = advertisements.Select(ad => new
            {
                ad.Id,
                ad.Title,
                ad.Description,
                ImageBase64 = DocumentSetting.GetBase64Image(ad.ImagePath, "advertisementImages")  // Assuming folder name is "ads"
            });

            return Ok(adsWithImages);

        }
        [Authorize]
        [HttpGet("dashboardData")]
        public async Task<IActionResult> DashboardData()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;

            if (currentUser == null)
                return Unauthorized();

            var appUser = await userManager.GetUserAsync(currentUser);

            if (appUser == null)
                return NotFound("User not found.");

            var result = new
            {
                Username = appUser.UserName,
                ImageBase64 = DocumentSetting.GetBase64Image(appUser.imagePath, "userImages")
            };

            return Ok(result);
        }
        //[HttpGet("Get-pharmacy")]
        //public async Task<IActionResult> pharmacy_filll()
        //{
        //    string query = @"
        //[out:json][timeout:30];
        //(
        //  node[""amenity""=""pharmacy""](29.9,30.9,30.1,31.3);
        //  way[""amenity""=""pharmacy""](29.9,30.9,30.1,31.3);
        //  relation[""amenity""=""pharmacy""](29.9,30.9,30.1,31.3);
        //);
        //out center;
        //";


        //    var content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");
        //    using var client = new HttpClient();
        //    var response = await client.PostAsync("https://overpass-api.de/api/interpreter", content);

        //    if (!response.IsSuccessStatusCode)
        //        return StatusCode((int)response.StatusCode, "Failed to fetch data from Overpass API");

        //    var jsonString = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<OverpassResponse>(jsonString);
        //    return Ok(result);
        //}

        [Authorize]
        [HttpGet("get-records")]
        public async Task<IActionResult> GetRecords()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in token.");

            var appUser = await userManager.GetUserAsync(currentUser);
            if (appUser == null)
                return NotFound("User not found.");

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetails(email);
            if (user == null)
                return NotFound("User not found in repository.");

            // Optional projection: customize what you want to return
            var recordsDto = user.Records.Select(record => new
            {
                record.Id,
                record.DoctorName,
                record.HospitalName,
                record.visitDate,
                record.Department,
            });

            return Ok(recordsDto);
        }



        [Authorize]
        [HttpGet("prescription")]
        public async Task<IActionResult> GetPrescription(int Id)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in token.");

            var appUser = await userManager.GetUserAsync(currentUser);
            if (appUser == null)
                return NotFound("User not found.");

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetails(email);
            if (user == null)
                return NotFound("User not found in repository.");


            var record = unitOfWork.appUsersRepository.GetRecord(Id);

            // Optional projection: customize what you want to return
            var medicines = record.prescription.medicines.Select(m => new
            {
                m.Id,
                m.Dosage,
                m.frequency,
                m.Duration,
                m.DoctorNotes,
                m.MedDate
            });

            return Ok(medicines);
        }

        [Authorize]
        [HttpGet("scans")]
        public async Task<IActionResult> get_scans(int Id)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in token.");

            var appUser = await userManager.GetUserAsync(currentUser);
            if (appUser == null)
                return NotFound("User not found.");

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetails(email);
            if (user == null)
                return NotFound("User not found in repository.");


            var record = unitOfWork.appUsersRepository.GetRecord(Id);

            // Optional projection: customize what you want to return
            var scans = record.Scans.Select(s => new
            {
                s.Type,
                s.Date,
                s.Description,
                ImageBase64 = DocumentSetting.GetBase64Image(s.ImagePath, "scanImage")  // Assuming folder name is "ads"

            });

            return Ok(scans);
        }

        [Authorize]
        [HttpGet("labtests")]
        public async Task<IActionResult> get_labtests(int Id)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in token.");

            var appUser = await userManager.GetUserAsync(currentUser);
            if (appUser == null)
                return NotFound("User not found.");

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetails(email);
            if (user == null)
                return NotFound("User not found in repository.");

            var record = unitOfWork.appUsersRepository.GetRecord(Id);
            if (record == null)
                return NotFound("Record not found.");

            var labTests = record.LabTests.Select(l => new
            {
                l.Name,
                l.Type,
                l.Notes,
                l.Date,
                ImageBase64 = DocumentSetting.GetBase64Image(l.ImagePath, "TestImages") 
            });

            return Ok(labTests);
        }


        [Authorize]
        [HttpGet("pharmacysearch")]
        public async Task<IActionResult> PharmacySearch([FromQuery] PharmacySearchViewModel pharmacySearchViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            pharmacySearchViewModel.Lat ??= 0;
            pharmacySearchViewModel.Lang ??= 0;

            var userLat = pharmacySearchViewModel.Lat.Value;
            var userLng = pharmacySearchViewModel.Lang.Value;

            double? maxDistance = pharmacySearchViewModel.Distance ??= 9999;

            var pharmacies = unitOfWork.pharamacyRepository.GetAll();

            var results = new List<PharmacyResultViewModel>();

            foreach (var pharmacy in pharmacies)
            {
                var pharmacyAcc = unitOfWork.pharmacyAccRepository.getpharmacyaccWithDetails(pharmacy.Identifier);
                
                double distance = 0;
                if (userLat > 0 && userLng > 0)
                {
                    if (!pharmacy.Latitude.HasValue || !pharmacy.Longitude.HasValue)
                        continue;

                    distance = DistanceCalc.CalculateHaversineDistance(userLat, userLng, pharmacy.Latitude.Value, pharmacy.Longitude.Value);

                    if (distance > maxDistance.Value)
                        continue;
                }

                results.Add(new PharmacyResultViewModel
                {
                    Name = pharmacy.Name,
                    Distance = distance,
                    pharmacyID = pharmacy.Identifier,
                    phonenumber = pharmacyAcc.PhoneNumber


                });
            }

            return Ok(results);
        }

        [Authorize]
        [HttpPost("medicine-request")]
        public async Task<IActionResult> MedicineRequest([FromBody] MedicineRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.MedicineName) || string.IsNullOrWhiteSpace(request.PharmacyId))
                return BadRequest("Medicine name and pharmacy ID are required.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var appUser = await userManager.GetUserAsync(currentUser);
            if (appUser == null)
                return NotFound("Authenticated user not found.");

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetails(email);
            if (user == null)
                return NotFound("User not found in system.");

            var pharmacyAcc = unitOfWork.pharmacyAccRepository.getpharmacyaccWithDetails(request.PharmacyId);
            if (pharmacyAcc == null)
                return NotFound("Pharmacy not found.");

            var req = new Request
            {
                MedicineName = request.MedicineName.Trim(),
                AppUser = user,
                SentAt = DateTime.UtcNow,
                pharmacyAcc = pharmacyAcc,
                qty = request.qty,
                state="pending",
                ClosedAt = null,
            };

            pharmacyAcc.Requests ??= new List<Request>();

            pharmacyAcc.Requests.Add(req);

            unitOfWork.Complete();
            return Ok("Request sent successfully.");
        }

        [Authorize("User")]
        [HttpGet("Notifications")]
        public async Task<IActionResult> Notifications()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { message = "User email not found." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetails(email);
            if (user == null)
                return NotFound(new { message = "User account not found." });

            var notifications = user.notifications;

            if (notifications == null || !notifications.Any())
            {
                return Ok(new
                {
                    message = "No notifications found.",
                    total = 0,
                    unread = 0,
                    notifications = new List<object>()
                });
            }

            var result = notifications
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Text,
                    n.CreatedAt,
                    n.IsRead,
                    n.Type
                }).ToList();

            var unreadCount = notifications.Count(n => !n.IsRead);

            return Ok(new
            {
                total = result.Count,
                unread = unreadCount,
                notifications = result
            });
        }
        [Authorize("User")]
        [HttpPost("Mark-notification-as-read")]
        public async Task<IActionResult> MarkNotificationAsRead([FromBody]int id)
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { message = "User email not found." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetails(email);
            if (user == null)
                return NotFound(new { message = "User account not found." });

            var notification = user.notifications?.FirstOrDefault(n => n.Id == id);
            if (notification == null)
                return NotFound(new { message = "Notification not found." });

            // Mark as read (if not already)
            if (!notification.IsRead)
            {
                notification.IsRead = true;
                unitOfWork.Complete(); // or await unitOfWork.CompleteAsync();
            }

            // Return full notification details
            return Ok(new
            {
                notification.Id,
                notification.Text,
                notification.CreatedAt,
                notification.IsRead,
                notification.Type,
                notification.PharmacyName,
                DisplayMessage = $"Notification from {notification.PharmacyName ?? "Hospital"} on {notification.CreatedAt:yyyy-MM-dd}"
            });
        }

        [Authorize("User")]
        [HttpPost("Mark-all-notifications-as-read")]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { message = "User email not found." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetails(email);
            if (user == null)
                return NotFound(new { message = "User account not found." });

            var unreadNotifications = user.notifications?.Where(n => !n.IsRead).ToList();
            if (unreadNotifications == null || !unreadNotifications.Any())
            {
                return Ok(new { message = "No unread notifications." });
            }

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            unitOfWork.Complete(); // or await unitOfWork.CompleteAsync();

            return Ok(new
            {
                message = $"Marked {unreadNotifications.Count} notification(s) as read."
            });
        }




    }
}
