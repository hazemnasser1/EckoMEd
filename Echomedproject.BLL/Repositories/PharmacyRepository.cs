using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Contexts;
using Echomedproject.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Echomedproject.BLL.Repositories
{
    public class PharmacyRepository : GenericRepository<Pharmacy>, IPharamacyRepository
    {
        EckomedDbContext dbContext;
        public PharmacyRepository(EckomedDbContext dbcontext) : base(dbcontext) {
        dbContext = dbcontext;
        }


        public async Task<Pharmacy?> FindAsync(Expression<Func<Pharmacy, bool>> predicate)
        {
            return await dbContext.pharmacies.FirstOrDefaultAsync(predicate);
        }

    }
}
