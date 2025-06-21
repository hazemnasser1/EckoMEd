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
    public class AppUsersRepository : GenericRepository<AppUsers>, IAppUsersRepository
    {
        EckomedDbContext dbContext;

        public AppUsersRepository(EckomedDbContext dbcontext) : base(dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public AppUsers getUserbyEmail(string email)
        {
            AppUsers user = dbContext.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }


        public AppUsers? getUserWithRecordDetails(string email)
        {
      
            var user = dbContext.Users
                .Include(u => u.Records)
                    .ThenInclude(r => r.prescription)
                .Include(u => u.Records)
                    .ThenInclude(r => r.Invoice)
                .Include(u => u.Records)
                    .ThenInclude(r => r.Scans)
                .Include(u => u.notifications)
                .FirstOrDefault(u => u.Email == email);


            return user;
        }

        public Records? GetRecord(int id)
        {
            return dbContext.Records
                .Include(r => r.Scans) // Include related Scan entity
                .Include(r => r.prescription)
                    .ThenInclude(p => p.medicines) // Include medicines inside prescription
                .FirstOrDefault(r => r.Id == id);
        }

    }
}
