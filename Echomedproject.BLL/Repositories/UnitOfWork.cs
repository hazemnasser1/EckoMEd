using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Contexts;
using Microsoft.Identity.Client;

namespace Echomedproject.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        EckomedDbContext dbContext;
        public IAppUsersRepository appUsersRepository { get; set; }
        public IDataEntryRepository entryRepository { get; set ; }
        public IDepartmentRepository departmentRepository { get ; set; }
        public IHospitalsRepository hospitalsRepository { get; set ; }
        public IInvoiceRepository invoiceRepository { get ; set ; }
        public ILabTestRepository labTestRepository { get; set; }
        public IPatientRepository patientRepository { get ; set; }
        public IPatientHospitalRepository patienthospitalRepository { get; set ; }
        public IPrescriptionRepository PrescriptionRepository { get; set; }
        public IRecordRepository recordRepository { get; set; }
        public IRoomRepository roomRepository { get; set; }
        public IScansRepository scansRepository { get ; set ; }
        public IUsersRepository usersRepository { get; set; }

        public UnitOfWork(EckomedDbContext dbContext)
        {
            appUsersRepository = new AppUsersRepository(dbContext);
            entryRepository = new DataEntryRepository(dbContext);
            departmentRepository = new DepartmentRepository(dbContext);
            hospitalsRepository = new HospitalsRepository(dbContext);
            invoiceRepository = new InvoiceRepository(dbContext);
            labTestRepository = new LabTestRepository(dbContext);
            patientRepository = new PatientRepository(dbContext);
            patienthospitalRepository = new PatientHospitalRepository(dbContext);
            PrescriptionRepository = new PrescriptionRepository(dbContext);
            recordRepository = new RecordRepository(dbContext); 
            roomRepository = new RoomRepository(dbContext);
            scansRepository = new ScansRepository(dbContext);
            usersRepository=new UserRepository(dbContext);

            this.dbContext = dbContext;

            


        }

        public int Complete()
        => dbContext.SaveChanges();

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
