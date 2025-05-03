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
    public class DataEntryRepository : GenericRepository<DataEntry>, IDataEntryRepository
    {
        

        public DataEntryRepository(EckomedDbContext dbcontext): base(dbcontext) { }
        
        
    }
}
