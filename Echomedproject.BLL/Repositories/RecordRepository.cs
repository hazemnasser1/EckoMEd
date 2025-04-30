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
    public class RecordRepository : IRecordRepository
    {
        private EckomedDbContext dbContext;

        public RecordRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(Records Records)
        {
            dbContext.Records.Add(Records);
            return dbContext.SaveChanges();
        }

        public int delete(Records Records)
        {
            dbContext.Records.Remove(Records);
            return dbContext.SaveChanges();
        }

        public Records Get(int id)
        {
            return dbContext.Records.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<Records> GetAll()
        {
            return dbContext.Records.ToList();
        }

        public int update(Records Records)
        {
            dbContext.Records.Update(Records);
            return dbContext.SaveChanges();
        }
    }
}
