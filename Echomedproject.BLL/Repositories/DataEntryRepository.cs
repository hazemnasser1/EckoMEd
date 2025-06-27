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
    public class DataEntryRepository : GenericRepository<DataEntry>, IDataEntryRepository
    {

        EckomedDbContext dbContext;
        public DataEntryRepository(EckomedDbContext dbcontext): base(dbcontext) { 
        dbContext = dbcontext;
        
        }

        public DataEntry getDataEntryWithDetails(string email)
        {
            var user = dbContext.dataEntry
                .Include(u => u.Departments) // Include related departments
                .FirstOrDefault(u => u.Email == email); // Search by email

            return user;
        }
    }
}
