using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Contexts;
using Echomedproject.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Echomedproject.BLL.Repositories
{
    public class PatientHospitalRepository :GenericRepository<PatientHospital>, IPatientHospitalRepository
    {
        EckomedDbContext dbContext;
        public PatientHospitalRepository(EckomedDbContext dbcontext) : base(dbcontext) {
        
        dbContext = dbcontext;
        }


        public PatientHospital GetPatientHospitalwithIDs(int hospitalID, string userID)
        {
            return dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID
                          && ph.PatientId == userID
                          && ph.LeaveDate == null)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.LabTests)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.notes)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.Scans)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.prescription)
                        .ThenInclude(p => p.medicines) // assuming prescription has a Medicines list
                .Include(ph => ph.record)
                    .ThenInclude(r => r.Invoice)
                .OrderByDescending(ph => ph.EntryDate)
                .FirstOrDefault();
        }





    }
}
