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
    public class DepartmentRepository : IDepartmentRepository
    {
        private EckomedDbContext dbContext;

        public DepartmentRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(Departments Departments)
        {
            dbContext.Departments.Add(Departments);
            return dbContext.SaveChanges();
        }

        public int delete(Departments Departments)
        {
            dbContext.Departments.Remove(Departments);
            return dbContext.SaveChanges();
        }

        public Departments Get(int id)
        {
            return dbContext.Departments.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<Departments> GetAll()
        {
            return dbContext.Departments.ToList();
        }

        public int update(Departments Departments)
        {
            dbContext.Departments.Update(Departments);
            return dbContext.SaveChanges();
        }
    }
}
