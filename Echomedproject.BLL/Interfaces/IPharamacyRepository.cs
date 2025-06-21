using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Migrations;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IPharamacyRepository : IGenericRepository<Pharmacy>
    {
        public Task<Pharmacy?> FindAsync(Expression<Func<Pharmacy, bool>> predicate);
    }
}
