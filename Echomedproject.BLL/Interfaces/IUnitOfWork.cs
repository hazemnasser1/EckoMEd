using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.BLL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
         IAppUsersRepository appUsersRepository { get; set; }
        IDataEntryRepository entryRepository { get; set; }
        IDepartmentRepository departmentRepository { get; set; }
        IHospitalsRepository hospitalsRepository { get; set; }
        IInvoiceRepository invoiceRepository { get; set; }
        ILabTestRepository labTestRepository { get; set; }
        IPatientRepository patientRepository { get; set; }
        IPatientHospitalRepository patienthospitalRepository { get; set; }
        IPrescriptionRepository PrescriptionRepository { get; set; }
        IRecordRepository recordRepository { get; set; }
        IRoomRepository roomRepository { get; set; }
        IScansRepository scansRepository { get; set; }
        IUsersRepository usersRepository { get; set; }
        IAdverismentRepository adverismentRepository { get; set; }
        IPharamacyRepository pharamacyRepository { get; set; }
        IPharmacyAccRepository pharmacyAccRepository { get; set; }
        INotificationRepository notificationRepository { get; set; }

        int Complete();

        void Dispose();

    }
}
