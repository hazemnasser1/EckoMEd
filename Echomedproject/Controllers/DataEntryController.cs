using System.Security.Claims;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Models;
using Echomedproject.PL.Helpers;
using Echomedproject.PL.ViewModels;
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
    }
}
