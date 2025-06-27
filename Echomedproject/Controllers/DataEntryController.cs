using System.Security.Claims;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Models;
using Echomedproject.PL.Helpers;
using Echomedproject.PL.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Echomedproject.PL.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
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
                return Unauthorized("User email not found.");

            var user = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (user == null)
                return NotFound("DataEntry account not found.");

            if (user.Departments == null || !user.Departments.Any())
                return Ok("No departments found for this user.");

            var departmentData = user.Departments.Select(dept => new
            {
                DepartmentName = dept.Name,
                TotalPatients = dept.TotalPatients
            }).ToList();

            return Ok(departmentData);
        }

        [HttpGet("FetchData")]
        public async Task<IActionResult> FetchData([FromQuery]string Id)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest("User ID is required.");

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);

            if (user == null)
                return NotFound("User not found.");

            // Build user data object

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
                Age= age,
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
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var user = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (user == null)
                return NotFound("DataEntry account not found.");

            // Make sure departments are loaded
            if (user.Departments == null || !user.Departments.Any())
                return NotFound("No departments found for the current user.");

            // Find department by name (case-insensitive match)
            var targetDepartment = user.Departments
                .FirstOrDefault(d => d.Name.Equals(addPatientViewModel.DepartmentName, StringComparison.OrdinalIgnoreCase));

            if (targetDepartment == null)
                return NotFound($"Department '{addPatientViewModel.DepartmentName}' not found.");

            // Access the TotalPatients parameter
            targetDepartment.TotalPatients+=1;
            PatientHospital patientHospital = new PatientHospital()
            {
                PatientId = addPatientViewModel.UserId,
                EntryDate = addPatientViewModel.DateTime,
                Department = addPatientViewModel.DepartmentName,
                HospitalId=user.HospitalID,
               

            };
            Records record = new Records()
            {
                Department = addPatientViewModel.DepartmentName,
                DoctorName = addPatientViewModel.DoctorName

            };


            patientHospital.record =record;

            unitOfWork.patienthospitalRepository.add(patientHospital);
            unitOfWork.Complete();
            return Ok(new
            {
                Message = "Patient added successfully.",
            });
        }

        [HttpGet("Medicines")]
        public async Task<IActionResult> Medicinces([FromQuery] string Id)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest("User ID is required.");

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);

            if (user == null)
                return NotFound("User not found.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (datauser == null)
                return NotFound("DataEntry account not found.");

            PatientHospital patientHospital = unitOfWork.patienthospitalRepository.GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound("Patient not found in the hospital.");



            return Ok(patientHospital.record.prescription.medicines.ToList());
        }

        [HttpGet("Scans")]
        public async Task<IActionResult> Scans([FromQuery] string Id)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest("User ID is required.");

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);

            if (user == null)
                return NotFound("User not found.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (datauser == null)
                return NotFound("DataEntry account not found.");

            PatientHospital patientHospital = unitOfWork.patienthospitalRepository.GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound("Patient not found in the hospital.");



            return Ok(patientHospital.record.Scans.ToList());
        }

        [HttpGet("Notes")]
        public async Task<IActionResult> Notes([FromQuery] string Id)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest("User ID is required.");

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);

            if (user == null)
                return NotFound("User not found.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (datauser == null)
                return NotFound("DataEntry account not found.");

            PatientHospital patientHospital = unitOfWork.patienthospitalRepository.GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound("Patient not found in the hospital.");



            return Ok(patientHospital.record.notes.ToList());
        }

        [HttpGet("LabTests")]
        public async Task<IActionResult> LabTests([FromQuery] string Id)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest("User ID is required.");

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);

            if (user == null)
                return NotFound("User not found.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (datauser == null)
                return NotFound("DataEntry account not found.");

            PatientHospital patientHospital = unitOfWork.patienthospitalRepository.GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound("Patient not found in the hospital.");



            return Ok(patientHospital.record.LabTests.ToList());
        }



        [HttpGet("PatientData")]
        public async Task<IActionResult> PatientData([FromQuery] string Id)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest("User ID is required.");

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(Id);

            if (user == null)
                return NotFound("User not found.");

            // Build user data object
            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);

            if (datauser == null)
                return NotFound("DataEntry account not found.");


            PatientHospital patientHospital = unitOfWork.patienthospitalRepository.GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);

            if(patientHospital==null)
                return NotFound("Patient not found in the hospital.");


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


        [HttpPost("Add-medicine")]
        public async Task<IActionResult> AddMedicine([FromBody] AddMedicineViewModel addMedicineViewModel)
        {
            // Validate ViewModel
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addMedicineViewModel.PatientID);
            if (user == null)
                return NotFound("User not found.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound("DataEntry account not found.");

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound("Patient not found in the hospital.");

            // Ensure prescription and medicines list are initialized
            if (patientHospital.record == null)
                return BadRequest("Patient record not found.");

            if (patientHospital.record.prescription == null)
                patientHospital.record.prescription = new prescription();

            if (patientHospital.record.prescription.medicines == null)
                patientHospital.record.prescription.medicines = new List<Medicine>();

            // Create and add medicine
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

            return Ok("Medicine added successfully.");
        }
        
        [HttpPost("Add-Note")]
        public async Task<IActionResult> addnote([FromBody] AddNoteViewModel addNoteViewModel)
        {
            // Validate ViewModel
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addNoteViewModel.PatientID);
            if (user == null)
                return NotFound("User not found.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound("DataEntry account not found.");

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound("Patient not found in the hospital.");

            if (patientHospital.record == null)
                return BadRequest("Patient record not found.");

            if (patientHospital.record.prescription == null)
                patientHospital.record.prescription = new prescription();

            if (patientHospital.record.prescription.medicines == null)
                patientHospital.record.prescription.medicines = new List<Medicine>();

            Note note = new Note()
            {
                Text = addNoteViewModel.NoteContent,
                type = addNoteViewModel.NoteType,
                dateTime = DateTime.Now

            };

            patientHospital.record.notes.Add(note);
            unitOfWork.Complete();

            return Ok("Note added successfully.");
        }

        [HttpPost("Add-Scan")]
        public async Task<IActionResult> addscan([FromForm] AddScanViewModel addScanViewModel)
        {
            // Validate ViewModel
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addScanViewModel.PatientID);
            if (user == null)
                return NotFound("User not found.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound("DataEntry account not found.");

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound("Patient not found in the hospital.");

            if (patientHospital.record == null)
                return BadRequest("Patient record not found.");

            if (patientHospital.record.prescription == null)
                patientHospital.record.prescription = new prescription();

            if (patientHospital.record.prescription.medicines == null)
                patientHospital.record.prescription.medicines = new List<Medicine>();

            string imagepath = DocumentSetting.UploadImage(addScanViewModel.Image, "scanImages");

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

            return Ok("Scan added successfully.");
        }

        [HttpPost("Add-Test")]
        public async Task<IActionResult> addTest([FromForm] AddTestViewModel addTestViewModel)
        {
            // Validate ViewModel
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get user with related data
            var user = unitOfWork.appUsersRepository.getUserWithRecordDetailsbyId(addTestViewModel.PatientID);
            if (user == null)
                return NotFound("User not found.");

            var currentUser = _httpContextAccessor?.HttpContext?.User;
            var email = currentUser?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found.");

            var datauser = unitOfWork.entryRepository.getDataEntryWithDetails(email);
            if (datauser == null)
                return NotFound("DataEntry account not found.");

            var patientHospital = unitOfWork.patienthospitalRepository
                .GetPatientHospitalwithIDs(datauser.HospitalID, user.UserName);
            if (patientHospital == null)
                return NotFound("Patient not found in the hospital.");

            if (patientHospital.record == null)
                return BadRequest("Patient record not found.");

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

            patientHospital.record.LabTests.Add(labTest);
            unitOfWork.Complete();

            return Ok("Scan added successfully.");
        }

    }
}
