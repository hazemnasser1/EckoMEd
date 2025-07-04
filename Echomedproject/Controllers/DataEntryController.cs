using System.Collections.Generic;
using System.Security.Claims;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Migrations;
using Echomedproject.DAL.Models;
using Echomedproject.PL.Helpers;
using Echomedproject.PL.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Echomedproject.PL.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "DataEntry")]

    public class DataEntryController : ControllerBase
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IUnitOfWork unitOfWork;
        public static string CurrentMemberEmail;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataEntryController(
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

        [HttpPost("add-record")]
        public async Task<IActionResult> add_record([FromForm] RecordViewModel recordViewModel)
        {
            if (ModelState.IsValid)
            {

                AppUsers user = unitOfWork.appUsersRepository.getUserWithRecordDetails(recordViewModel.Email);

                if (user == null)
                    return NotFound("User not found.");

                Records record = new Records()
                {
                    DoctorName = recordViewModel.doctorName,
                    HospitalName = recordViewModel.doctorName,
                    visitDate = recordViewModel.date,
                    Department = recordViewModel.departmentName,
                    Scans = new List<Scans>(),
                    prescription = new prescription(),

                };
                Scans scan = new Scans()
                {
                    Type = recordViewModel.scanName,
                    Description= recordViewModel.scanDescription,
                    Date = recordViewModel.date

                };
                if (recordViewModel.ScanImage != null)
                {
                    string imagepath = DocumentSetting.UploadImage(recordViewModel.ScanImage, "scanImage");

                    scan.ImagePath = imagepath;
                }
                record.Scans.Add(scan);
                record.prescription.medicines ??= new List<Medicine>();

                foreach (var _medicine in recordViewModel.Medicines)
                {
                    Medicine medicine = new Medicine()
                    {
                        Name=_medicine.name,
                        DoctorNotes = _medicine.doctorNotes,
                        Dosage = _medicine.dosage,
                        frequency = _medicine.frequency,
                        MedDate = recordViewModel.date,
                        Duration = _medicine.duration


                    };


                    record.prescription.medicines.Add(medicine);

                }

                    

                user.Records.Add(record);
                unitOfWork.Complete();
                
                

                return Ok("Record added Successfully.");
            }
            return BadRequest("Data is not Valid");
        }

        //[HttpGet("test")]
        //public async Task<IActionResult> test()
        //{
        //    var dataentries = unitOfWork.entryRepository.GetAll().ToList();
        //    var departments = unitOfWork.departmentRepository.GetAll().ToList();

        //    foreach (var entry in dataentries)
        //    {
        //        // Find departments with matching HospitalId
        //        var matchingDepartments = departments
        //            .Where(dep => dep.HospitalID == entry.HospitalID)
        //            .ToList();

        //        // Assuming the entry has a property: List<Department> Departments
        //        entry.Departments = matchingDepartments;
        //    }

        //    unitOfWork.Complete();

        //    return Ok("Departments assigned successfully to data entries.");
        //}

        [HttpGet("Capacity")]
        public async Task<IActionResult> Capacity()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var user = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (user == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            if (user.Departments == null || !user.Departments.Any())
                return Ok(new { message = "No departments found for this user." });

            var departmentData = user.Departments.Select(dept => new
            {
                DepartmentName = dept.Name,
                TotalPatients = dept.TotalPatients
            }).ToList();

            return Ok(departmentData);
        }

        [HttpGet("FetchData")]
        public async Task<IActionResult> FetchData([FromQuery] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest(new { success = false, message = "User ID is required." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);

            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            int? age = null;
            if (user.DateOfBirth != null)
            {
                var today = DateTime.Today;
                var birthDate = user.DateOfBirth.Value;
                age = today.Year - birthDate.Year;
                if (birthDate > today.AddYears(-age.Value)) age--;
            }

            var userData = new
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Age = age,
                PhoneNumber = user.PhoneNum,
                Email = user.Email,
                Address = user.street,
                City = user.City,
            };

            return Ok(userData);
        }


        [HttpPost("AddPatient")]
        public async Task<IActionResult> AddPatient([FromBody] AddPatientViewModel addPatientViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var user = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (user == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            if (user.Hospital.Departments == null || !user.Hospital.Departments.Any())
                return NotFound(new { success = false, message = "No departments found for the current user." });

            var targetDepartment = user.Hospital.Departments
                .FirstOrDefault(d => d.Name.Equals(addPatientViewModel.DepartmentName, StringComparison.OrdinalIgnoreCase));

            if (targetDepartment == null)
                return NotFound(new { success = false, message = $"Department '{addPatientViewModel.DepartmentName}' not found." });

            var appuser = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addPatientViewModel.UserId);
            if (appuser == null)
                return NotFound(new { success = false, message = "User not found." });

            PatientHospital patient = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(user.HospitalID, addPatientViewModel.UserId);

            if (patient != null && patient.LeaveDate == null)
                return Conflict(new { success = false, message = "User is already admitted in the hospital." });

            PatientHospital patientHospital = new PatientHospital()
            {
                PatientId = addPatientViewModel.UserId,
                EntryDate = addPatientViewModel.DateTime,
                Department = addPatientViewModel.DepartmentName,
                HospitalId = user.HospitalID,
                DateOfbirth = appuser.DateOfBirth,
                Gender = appuser.Gender,
                patientName = appuser.FirstName + " " + appuser.LastName,
                State = "Pending"
            };

            Records record = new Records()
            {
                Department = addPatientViewModel.DepartmentName,
                DoctorName = addPatientViewModel.DoctorName,
                HospitalName = user.Hospital?.Name ?? "Unknown Hospital",
                visitDate = DateTime.Now,
                prescription = new prescription(),
                LabTests = new List<LabTest>(),
                Scans = new List<Scans>(),
            };

            record.prescription.medicines = new List<Medicine>();
            patientHospital.record = record;

            user.TotalPatients += 1;
            targetDepartment.TotalPatients += 1;

            unitOfWork.patienthospitalRepository.add(patientHospital);
            unitOfWork.Complete();

            return Ok(new { success = true, message = "Patient added successfully." });
        }

        [HttpGet("Medicines")]
        public async Task<IActionResult> Medicinces([FromQuery] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest(new { success = false, message = "User ID is required." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            PatientHospital patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);

            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            return Ok(patientHospital.record.prescription.medicines.ToList());
        }


        [HttpGet("Scans")]
        public async Task<IActionResult> Scans([FromQuery] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest(new { success = false, message = "User ID is required." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository.GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var scans = patientHospital.record.Scans;

            var scanResponses = scans.Select(scan => new
            {
                scan.Id,
                scan.Type,
                scan.bodypart,
                scan.Description,
                scan.Date,
                ImageBase64 = $"{baseUrl}/files/ScanImages/{scan.ImagePath}"
            }).ToList();

            return Ok(scanResponses);
        }

        [HttpGet("Notes")]
        public async Task<IActionResult> Notes([FromQuery] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest(new { success = false, message = "User ID is required." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository.GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            return Ok(patientHospital.record.notes.ToList());
        }

        [HttpGet("LabTests")]
        public async Task<IActionResult> LabTests([FromQuery] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest(new { success = false, message = "User ID is required." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository.GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            var labTests = patientHospital.record.LabTests;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var labTestResponses = labTests.Select(test => new
            {
                test.Id,
                test.Name,
                test.Type,
                test.Notes,
                test.Date,
                ImageBase64 = $"{baseUrl}/files/TestImages/{test.ImagePath}"
            }).ToList();

            return Ok(labTestResponses);
        }





        [HttpGet("PatientData")]
        public async Task<IActionResult> PatientData([FromQuery] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest(new { success = false, message = "User ID is required." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);

            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            int labTestsCount = patientHospital?.record?.LabTests?.Count() ?? 0;
            int medicineCount = patientHospital?.record?.prescription?.medicines?.Count() ?? 0;
            int notesCount = patientHospital?.record?.notes?.Count() ?? 0;
            int scansCount = patientHospital?.record?.Scans?.Count() ?? 0;

            int? age = null;
            if (user.DateOfBirth != null)
            {
                var today = DateTime.Today;
                var birthDate = user.DateOfBirth.Value;
                age = today.Year - birthDate.Year;
                if (birthDate > today.AddYears(-age.Value)) age--;
            }

            var userData = new
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Age = age,
                PhoneNumber = user.PhoneNum,
                Email = user.Email,
                Address = user.street,
                City = user.City,

                LabTestsCount = labTestsCount,
                MedicineCount = medicineCount,
                NotesCount = notesCount,
                ScansCount = scansCount
            };

            return Ok(userData);
        }

        [HttpPost("Add-medicine")]
        public async Task<IActionResult> AddMedicine([FromBody] AddMedicineViewModel addMedicineViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addMedicineViewModel.PatientID);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            if (patientHospital.record == null)
                return BadRequest(new { success = false, message = "Patient record not found." });

            if (patientHospital.record.prescription == null)
                patientHospital.record.prescription = new prescription();

            if (patientHospital.record.prescription.medicines == null)
                patientHospital.record.prescription.medicines = new List<Medicine>();

            var medicine = new Medicine
            {
                Name = addMedicineViewModel.MedicineName,
                MedDate = DateTime.Now,
                DoctorNotes = addMedicineViewModel.MedicineNotes,
                Dosage = addMedicineViewModel.Dosage,
                Duration = addMedicineViewModel.Duration,
                frequency = addMedicineViewModel.Frequency,
                Timing = addMedicineViewModel.Timing
            };

            patientHospital.record.prescription.medicines.Add(medicine);
            unitOfWork.Complete();

            return Ok(new { success = true, message = "Medicine added successfully" });
        }

        [HttpPost("Add-Note")]
        public async Task<IActionResult> addnote([FromBody] AddNoteViewModel addNoteViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addNoteViewModel.PatientID);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            if (patientHospital.record == null)
                return BadRequest(new { success = false, message = "Patient record not found." });

            if (patientHospital.record.notes == null)
                patientHospital.record.notes = new List<Note>();

            Note note = new Note()
            {
                Text = addNoteViewModel.NoteContent,
                type = addNoteViewModel.NoteType,
                dateTime = DateTime.Now
            };

            patientHospital.record.notes.Add(note);
            unitOfWork.Complete();

            return Ok(new { success = true, message = "Note added successfully" });
        }


        [HttpPost("Add-Scan")]
        public async Task<IActionResult> addscan([FromForm] AddScanViewModel addScanViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addScanViewModel.PatientID);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            if (patientHospital.record == null)
                return BadRequest(new { success = false, message = "Patient record not found." });

            if (patientHospital.record.prescription == null)
                patientHospital.record.prescription = new prescription();

            if (patientHospital.record.prescription.medicines == null)
                patientHospital.record.prescription.medicines = new List<Medicine>();

            string imagepath = DocumentSetting.UploadImage(addScanViewModel.Image, "ScanImages");

            Scans scans = new Scans()
            {
                Type = addScanViewModel.ScanType,
                bodypart = addScanViewModel.ScanPart,
                Date = DateTime.Now,
                ImagePath = imagepath,
                Description = addScanViewModel.Note
            };

            patientHospital.record.Scans.Add(scans);
            unitOfWork.Complete();

            return Ok(new { success = true, message = "Scan added successfully" });
        }

        [HttpPost("Add-Test")]
        public async Task<IActionResult> addTest([FromForm] AddTestViewModel addTestViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addTestViewModel.PatientID);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            if (patientHospital.record == null)
                return BadRequest(new { success = false, message = "Patient record not found." });

            if (patientHospital.record.prescription == null)
                patientHospital.record.prescription = new prescription();

            if (patientHospital.record.prescription.medicines == null)
                patientHospital.record.prescription.medicines = new List<Medicine>();

            string imagepath = DocumentSetting.UploadImage(addTestViewModel.Image, "TestImages");

            LabTest labTest = new LabTest()
            {
                Name = addTestViewModel.TestName,
                Type = addTestViewModel.TestType,
                ImagePath = imagepath,
                Date = DateTime.Now,
                Notes = addTestViewModel.Note,
            };

            datauser.LabTestCount += 1;

            patientHospital.record.LabTests.Add(labTest);
            unitOfWork.Complete();

            return Ok(new { success = true, message = "TestLab added successfully" });
        }

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut([FromForm] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest(new { success = false, message = "User ID is required." });

            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);
            if (user == null)
                return NotFound(new { success = false, message = "User not found." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound(new { success = false, message = "Patient not found in the hospital." });

            patientHospital.LeaveDate = DateTime.Now;
            string departmentName = patientHospital.Department;
            var department = datauser.Hospital.Departments
                .FirstOrDefault(d => d.Name.Equals(departmentName, StringComparison.OrdinalIgnoreCase));

            if (department != null)
            {
                datauser.TotalPatients -= 1;
                department.TotalPatients -= 1;
            }

            AppUsers appuser = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);
            appuser.Records.Add(patientHospital.record);
            unitOfWork.patienthospitalRepository.update(patientHospital);
            unitOfWork.Complete();

            return Ok(new { success = true, message = "Patient checkedout successfully" });
        }


        [HttpGet("Departments")]
        public async Task<IActionResult> Departments()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            if (datauser.Hospital == null || datauser.Hospital.Departments == null)
                return NotFound(new { success = false, message = "No departments found for this user's hospital." });

            var departments = datauser.Hospital.Departments
                .Select(dep => new
                {
                    Name = dep.Name,
                    Description = dep.Description,
                    capacity = dep.Capacity,
                    numOfDoctors = dep.NomOfDoctors
                })
                .ToList();

            return Ok(departments);
        }

        [HttpGet("departmentnames")]
        public async Task<IActionResult> Departmentnames()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            if (datauser.Hospital == null || datauser.Hospital.Departments == null)
                return NotFound(new { success = false, message = "No departments found for this user's hospital." });

            var departments = datauser.Hospital.Departments
                .Select(dep => new
                {
                    Name = dep.Name,
                })
                .ToList();

            return Ok(departments);
        }

        [HttpGet("DepartmentPatients")]
        public async Task<IActionResult> DepartmentPatients([FromQuery] string Department)
        {
            if (string.IsNullOrWhiteSpace(Department))
                return BadRequest(new { success = false, message = "Department name is required." });

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var patientHospitals = unitOfWork.patienthospitalRepository
                .GetPatientsByDepartmentAndHospital(Department, datauser.HospitalID);

            var patientList = patientHospitals.Select(ph =>
            {
                int? age = null;
                if (ph?.DateOfbirth != null)
                {
                    var today = DateTime.Today;
                    var birthDate = ph.DateOfbirth.Value;
                    age = today.Year - birthDate.Year;
                    if (birthDate > today.AddYears(-age.Value)) age--;
                }

                return new
                {
                    UserName = ph.PatientId,
                    Name = ph.patientName,
                    Gender = ph?.Gender,
                    Age = age,
                    Waiting = 0
                };
            }).ToList();

            return Ok(patientList);
        }



        [HttpGet("DashboardWeakly")]
        public async Task<IActionResult> DashboardWeakly()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            if (datauser.Hospital == null || datauser.Hospital.Departments == null)
                return NotFound(new { success = false, message = "No departments found for this user's hospital." });

            var hospitalId = datauser.HospitalID;

            // Weekly data
            var totalPatientsData = unitOfWork.patienthospitalRepository.GetWeeklyPatientCounts(hospitalId);
            var maleData = unitOfWork.patienthospitalRepository.GetWeeklyMalePatientCounts(hospitalId);
            var femaleData = unitOfWork.patienthospitalRepository.GetWeeklyFemalePatientCounts(hospitalId);
            var labTestsData = unitOfWork.patienthospitalRepository.GetLabTestCountsByWeek(hospitalId);

            int currentPatients = (int)totalPatientsData.GetType().GetProperty("CurrentWeekPatients").GetValue(totalPatientsData);
            int lastPatients = (int)totalPatientsData.GetType().GetProperty("LastWeekPatients").GetValue(totalPatientsData);
            int currentMales = (int)maleData.GetType().GetProperty("CurrentWeekMales").GetValue(maleData);
            int lastMales = (int)maleData.GetType().GetProperty("LastWeekMales").GetValue(maleData);
            int currentFemales = (int)femaleData.GetType().GetProperty("CurrentWeekFemales").GetValue(femaleData);
            int lastFemales = (int)femaleData.GetType().GetProperty("LastWeekFemales").GetValue(femaleData);
            int currentLabTests = (int)labTestsData.GetType().GetProperty("CurrentWeekLabTests").GetValue(labTestsData);
            int lastLabTests = (int)labTestsData.GetType().GetProperty("LastWeekLabTests").GetValue(labTestsData);

            string CalcPercentLabel(int last, int current)
            {
                if (last == 0)
                    return current > 0 ? "+100%" : "0%";
                double ratio = ((double)current - last) / last;
                double percent = Math.Round(Math.Abs(ratio * 100));
                if (percent == 0) return "0%";
                return $"{(ratio > 0 ? "+" : "-")}{percent}%";
            }

            var currentDaily = unitOfWork.patienthospitalRepository.GetDailyPatientCountsForWeek(hospitalId);
            var lastDaily = unitOfWork.patienthospitalRepository.GetDailyPatientCountsForLastWeek(hospitalId);

            return Ok(new
            {
                DataEntryName = datauser.Hospital.Name,
                CurrentWeek = new
                {
                    TotalPatients = currentPatients,
                    MalePatients = currentMales,
                    FemalePatients = currentFemales,
                    LabTests = currentLabTests
                },
                ChangeFromLastWeek = new
                {
                    TotalPatientsPercent = CalcPercentLabel(lastPatients, currentPatients),
                    MalePatientsPercent = CalcPercentLabel(lastMales, currentMales),
                    FemalePatientsPercent = CalcPercentLabel(lastFemales, currentFemales),
                    LabTestsPercent = CalcPercentLabel(lastLabTests, currentLabTests)
                },
                DailyBreakdown = new
                {
                    CurrentWeek = currentDaily,
                    LastWeek = lastDaily
                }
            });
        }

        [HttpGet("DashboardMonthly")]
        public async Task<IActionResult> DashboardMonthly()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            if (datauser.Hospital == null || datauser.Hospital.Departments == null)
                return NotFound(new { success = false, message = "No departments found for this user's hospital." });

            var hospitalId = datauser.HospitalID;

            // Monthly data
            var totalPatientsData = unitOfWork.patienthospitalRepository.GetMonthlyPatientCounts(hospitalId);
            var maleData = unitOfWork.patienthospitalRepository.GetMonthlyMalePatientCounts(hospitalId);
            var femaleData = unitOfWork.patienthospitalRepository.GetMonthlyFemalePatientCounts(hospitalId);
            var labTestsData = unitOfWork.patienthospitalRepository.GetLabTestCountsByMonth(hospitalId);

            int currentPatients = (int)totalPatientsData.GetType().GetProperty("CurrentMonthPatients").GetValue(totalPatientsData);
            int lastPatients = (int)totalPatientsData.GetType().GetProperty("LastMonthPatients").GetValue(totalPatientsData);
            int currentMales = (int)maleData.GetType().GetProperty("CurrentMonthMales").GetValue(maleData);
            int lastMales = (int)maleData.GetType().GetProperty("LastMonthMales").GetValue(maleData);
            int currentFemales = (int)femaleData.GetType().GetProperty("CurrentMonthFemales").GetValue(femaleData);
            int lastFemales = (int)femaleData.GetType().GetProperty("LastMonthFemales").GetValue(femaleData);
            int currentLabTests = (int)labTestsData.GetType().GetProperty("CurrentMonthLabTests").GetValue(labTestsData);
            int lastLabTests = (int)labTestsData.GetType().GetProperty("LastMonthLabTests").GetValue(labTestsData);

            string CalcPercentLabel(int last, int current)
            {
                if (last == 0)
                    return current > 0 ? "+100%" : "0%";
                double ratio = ((double)current - last) / last;
                double percent = Math.Round(Math.Abs(ratio * 100));
                if (percent == 0) return "0%";
                return $"{(ratio > 0 ? "+" : "-")}{percent}%";
            }

            var currentDaily = unitOfWork.patienthospitalRepository.GetDailyPatientCountsForCurrentMonth(hospitalId);
            var lastDaily = unitOfWork.patienthospitalRepository.GetDailyPatientCountsForLastMonth(hospitalId);

            return Ok(new
            {
                DataEntryName = datauser.Hospital.Name,
                CurrentMonth = new
                {
                    TotalPatients = currentPatients,
                    MalePatients = currentMales,
                    FemalePatients = currentFemales,
                    LabTests = currentLabTests
                },
                ChangeFromLastMonth = new
                {
                    TotalPatientsPercent = CalcPercentLabel(lastPatients, currentPatients),
                    MalePatientsPercent = CalcPercentLabel(lastMales, currentMales),
                    FemalePatientsPercent = CalcPercentLabel(lastFemales, currentFemales),
                    LabTestsPercent = CalcPercentLabel(lastLabTests, currentLabTests)
                },
                DailyBreakdown = new
                {
                    CurrentMonth = currentDaily,
                    LastMonth = lastDaily
                }
            });
        }

        [HttpGet("DashboardWeeklyGender")]
        public async Task<IActionResult> DashboardWeeklyGender()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound("DataEntry account not found.");

            var hospitalId = datauser.HospitalID;

            var maleData = unitOfWork.patienthospitalRepository.GetWeeklyMalePatientCounts(hospitalId);
            var femaleData = unitOfWork.patienthospitalRepository.GetWeeklyFemalePatientCounts(hospitalId);

            int currentMales = (int)maleData.GetType().GetProperty("CurrentWeekMales").GetValue(maleData);
            int lastMales = (int)maleData.GetType().GetProperty("LastWeekMales").GetValue(maleData);

            int currentFemales = (int)femaleData.GetType().GetProperty("CurrentWeekFemales").GetValue(femaleData);
            int lastFemales = (int)femaleData.GetType().GetProperty("LastWeekFemales").GetValue(femaleData);

            string CalcPercentLabel(int last, int current)
            {
                if (last == 0)
                    return current > 0 ? "+100%" : "0%";

                double ratio = ((double)current - last) / last;
                double percent = Math.Abs(ratio * 100);
                string sign = ratio > 0 ? "+" : "-";

                return $"{sign}{Math.Round(percent)}%";
            }

            return Ok(new
            {
                MalePatients = new
                {
                    Count = currentMales,
                    PercentChange = CalcPercentLabel(lastMales, currentMales)
                },
                FemalePatients = new
                {
                    Count = currentFemales,
                    PercentChange = CalcPercentLabel(lastFemales, currentFemales)
                }
            });
        }

        [HttpGet("DashboardMonthlyGender")]
        public async Task<IActionResult> DashboardMonthlyGender()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound("DataEntry account not found.");

            var hospitalId = datauser.HospitalID;

            var maleData = unitOfWork.patienthospitalRepository.GetMonthlyMalePatientCounts(hospitalId);
            var femaleData = unitOfWork.patienthospitalRepository.GetMonthlyFemalePatientCounts(hospitalId);

            int currentMales = (int)maleData.GetType().GetProperty("CurrentMonthMales").GetValue(maleData);
            int lastMales = (int)maleData.GetType().GetProperty("LastMonthMales").GetValue(maleData);

            int currentFemales = (int)femaleData.GetType().GetProperty("CurrentMonthFemales").GetValue(femaleData);
            int lastFemales = (int)femaleData.GetType().GetProperty("LastMonthFemales").GetValue(femaleData);

            string CalcPercentLabel(int last, int current)
            {
                if (last == 0)
                    return current > 0 ? "+100%" : "0%";

                double ratio = ((double)current - last) / last;
                double percent = Math.Abs(ratio * 100);
                string sign = ratio > 0 ? "+" : "-";

                return $"{sign}{Math.Round(percent)}%";
            }

            return Ok(new
            {
                MalePatients = new
                {
                    Count = currentMales,
                    PercentChange = CalcPercentLabel(lastMales, currentMales)
                },
                FemalePatients = new
                {
                    Count = currentFemales,
                    PercentChange = CalcPercentLabel(lastFemales, currentFemales)
                }
            });
        }

        [HttpGet("DataEntryprofile")]
        public async Task<IActionResult> DataEntryprofile()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var identityUser = await userManager.FindByEmailAsync(email);
            if (identityUser == null)
                return NotFound(new { success = false, message = "Identity user not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            var profileData = new
            {
                FirstName = identityUser.FirstName,
                LastName = identityUser.LastName,
                PhoneNumber = identityUser.PhoneNumber,
                email = identityUser.Email,
                HospitalName = datauser.Hospital?.Name,
                City = datauser.Hospital?.City
            };

            return Ok(profileData);
        }


        [HttpPost("UpdateDataEntryProfile")]
        public async Task<IActionResult> UpdateDataEntryProfile([FromBody] UpdateDataEntryProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var identityUser = await userManager.FindByEmailAsync(email);
            if (identityUser == null)
                return NotFound(new { success = false, message = "Identity user not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            identityUser.FirstName = model.FirstName;
            identityUser.LastName = model.LastName;
            identityUser.PhoneNumber = model.PhoneNumber;

            var identityResult = await userManager.UpdateAsync(identityUser);
            if (!identityResult.Succeeded)
                return BadRequest(new { success = false, message = "Failed to update identity user." });

            datauser.PhoneNumber = model.PhoneNumber;
            unitOfWork.Complete();

            return Ok(new { success = true, message = "Profile updated successfully." });
        }


        [HttpPost("UpdateCapacity")]
        public async Task<IActionResult> UpdateCapacity([FromBody] CapacityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "User email not found." });

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound(new { success = false, message = "DataEntry account not found." });

            if (datauser.Hospital == null || datauser.Hospital.Departments == null)
                return NotFound(new { success = false, message = "No departments found for this user's hospital." });

            var department = datauser.Hospital.Departments
                .FirstOrDefault(d => d.Name.Equals(model.DepartmentName, StringComparison.OrdinalIgnoreCase));

            if (department == null)
                return NotFound(new { success = false, message = $"Department '{model.DepartmentName}' not found." });

            // ✅ Update department fields
            department.Capacity = model.Capacity;
            department.NomOfDoctors = model.NumOfDoctors;

            unitOfWork.Complete();

            return Ok(new { success = true, message = "Department capacity updated successfully." });
        }

    }
}
