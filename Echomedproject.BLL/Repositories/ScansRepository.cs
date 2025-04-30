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
    public class ScansRepository : IScansRepository
    {
        private EckomedDbContext dbContext;

        public ScansRepository (EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        } 
        public int add(Scans Scans)
        {
            dbContext.Scans.Add(Scans);
            return dbContext.SaveChanges();
        }

        public int delete(Scans Scans)
        {
            dbContext.Scans.Remove(Scans);
            return dbContext.SaveChanges();
        }

        public Scans Get(int id)
        {
            return dbContext.Scans.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<Scans> GetAll()
        {
            return dbContext.Scans.ToList();
        }

        public int update(Scans Scans)
        {
            dbContext.Scans.Update(Scans);
            return dbContext.SaveChanges();
        }
    }
}
