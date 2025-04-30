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
    public class HospitalsRepository : IHospitalsRepository
    {
        private EckomedDbContext dbContext;

        public HospitalsRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(Hospitals hospital)
        {
            dbContext.hospitals.Add(hospital);
            return dbContext.SaveChanges();
        }

        public int delete(Hospitals Hospitals)
        {
            dbContext.hospitals.Remove(Hospitals);
            return dbContext.SaveChanges();
        }

        public Hospitals Get(int id)
        {
            return dbContext.hospitals.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<Hospitals> GetAll()
        {
            return dbContext.hospitals.ToList();
        }

        public int update(Hospitals hospitals)
        {
            dbContext.hospitals.Update(hospitals);
            return dbContext.SaveChanges();
        }
    }
}
