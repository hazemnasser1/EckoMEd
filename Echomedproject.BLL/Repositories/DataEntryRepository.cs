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
    public class DataEntryRepository : IDataEntryRepository
    {
        private EckomedDbContext dbContext;

        public DataEntryRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(DataEntry DataEntry)
        {
            dbContext.dataEntry.Add(DataEntry);
            return dbContext.SaveChanges();

        }

        public int delete(DataEntry DataEntry)
        {
           dbContext.dataEntry.Remove(DataEntry);
            return dbContext.SaveChanges();
        }

        public DataEntry Get(int id)
        {
            return dbContext.dataEntry.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<DataEntry> GetAll()
        {
            return dbContext.dataEntry.ToList();
        }

        public int update(DataEntry DataEntry)
        {
            dbContext.dataEntry.Update(DataEntry);
            return dbContext.SaveChanges();
        }
    }
}
