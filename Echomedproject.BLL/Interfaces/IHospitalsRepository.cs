using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IHospitalsRepository: IGenericRepository<Hospitals>
    {
        public  Task<List<Hospitals>> GetAllHospitalsWithDetailsAsync();
        public Task<Hospitals?> FindAsync(Expression<Func<Hospitals, bool>> predicate);

    }
}
