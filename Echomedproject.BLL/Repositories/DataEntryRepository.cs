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
                .Include(u => u.Departments) // Direct departments of the data entry (if any)
                .Include(u => u.Hospital)    // Include the related Hospital
                    .ThenInclude(h => h.Departments) // Include Departments of the Hospital
                .FirstOrDefault(u => u.Email == email);

            return user;
        }

    }
}
