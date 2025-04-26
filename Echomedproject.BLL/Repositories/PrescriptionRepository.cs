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
    internal class PrescriptionRepository : IPrescriptionRepository
    {
        private EckomedDbContext dbContext;

        public PrescriptionRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(prescription prescription)
        {
            dbContext.prescriptions.Add(prescription);
            return dbContext.SaveChanges();
        }

        public int delete(prescription prescription)
        {
           dbContext.prescriptions.Remove(prescription);
            return dbContext.SaveChanges();
        }

        public prescription Get(int id)
        {
            return dbContext.prescriptions.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<prescription> GetAll()
        {
            return dbContext.prescriptions.ToList();
        }

        public int update(prescription prescription)
        {
            dbContext.prescriptions.Update(prescription);
            return dbContext.SaveChanges();
        }
    }
}
