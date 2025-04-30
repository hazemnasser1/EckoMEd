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
    public class LabTestRepository : ILabTestRepository
    {
        private EckomedDbContext dbContext;

        public LabTestRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(LabTest LabTest)
        {
            dbContext.labTests.Add(LabTest);
            return dbContext.SaveChanges();
        }

        public int delete(LabTest LabTest)
        {
            dbContext.labTests.Remove(LabTest);
            return dbContext.SaveChanges();
        }

        public LabTest Get(int id)
        {
            return dbContext.labTests.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<LabTest> GetAll()
        {
            return dbContext.labTests.ToList();
        }

        public int update(LabTest LabTest)
        {
            dbContext.labTests.Update(LabTest);
            return dbContext.SaveChanges();
        }
    }
}
