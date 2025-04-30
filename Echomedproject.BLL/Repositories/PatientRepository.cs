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
    public class PatientRepository : IPatientRepository
    {
        private EckomedDbContext dbContext;

        public PatientRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(Patients Patients)
        {
            dbContext.Patients.Add(Patients);
            return dbContext.SaveChanges();
        }

        public int delete(Patients Patients)
        {
            dbContext.Patients.Remove(Patients);
            return dbContext.SaveChanges();
        }

        public Patients Get(int id)
        {
            return dbContext.Patients.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<Patients> GetAll()
        {
            return dbContext.Patients.ToList();
        }

        public int update(Patients Patients)
        {
            dbContext.Patients.Update(Patients);
            return dbContext.SaveChanges();
        }
    }
}
