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
    public class PharmacyAccRepository:GenericRepository<PharmacyAcc>, IPharmacyAccRepository
    {
        EckomedDbContext dbContext;
        public PharmacyAccRepository(EckomedDbContext dbcontext) : base(dbcontext) { 
        dbContext=dbcontext;
        }

        public PharmacyAcc? getpharmacyaccWithDetails(string pharmacyID)
        {
            var user = dbContext.pharmacyAccs
                .Include(p => p.Pharmacy) // Include the related Pharmacy
                .Include(p => p.Requests) // Include the related Requests
                    .ThenInclude(r => r.AppUser) // Include AppUser inside each Request
                .FirstOrDefault(p => p.Username == pharmacyID);

            return user;
        }


    }
}
