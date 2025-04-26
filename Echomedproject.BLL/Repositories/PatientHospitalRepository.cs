using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Contexts;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Repositories
{
    internal class PatientHospitalRepository : IPatientHospitalRepository
    {
        private EckomedDbContext dbContext;

        public PatientHospitalRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(PatientHospital PatientHospital)
        {
           dbContext.patientHospital.Add(PatientHospital);
            return dbContext.SaveChanges();
        }

        public int delete(PatientHospital PatientHospital)
        {
            dbContext.patientHospital.Remove(PatientHospital);
            return dbContext.SaveChanges();
        }

        public PatientHospital Get(int id)
        {
            return dbContext.patientHospital.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<PatientHospital> GetAll()
        {
            return dbContext.patientHospital.ToList();
        }

        public int update(PatientHospital PatientHospital)
        {
            dbContext.patientHospital.Update(PatientHospital);
            return dbContext.SaveChanges();
        }
    }
}
