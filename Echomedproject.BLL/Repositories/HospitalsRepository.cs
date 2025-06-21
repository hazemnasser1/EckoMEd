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
    public class HospitalsRepository :GenericRepository<Hospitals>, IHospitalsRepository
    {
        EckomedDbContext dbContext;

        public HospitalsRepository(EckomedDbContext dbcontext): base(dbcontext) {
            this.dbContext = dbcontext;

        }

        public async Task<List<Hospitals>> GetAllHospitalsWithDetailsAsync()
        {
            return await dbContext.hospitals
                .Include(h => h.Departments)
                .Include(h => h.AcceptedInsurances)
                .ToListAsync();
        }
        public async Task<Hospitals?> FindAsync(Expression<Func<Hospitals, bool>> predicate)
        {
            return await dbContext.hospitals.FirstOrDefaultAsync(predicate);
        }

    }
}
