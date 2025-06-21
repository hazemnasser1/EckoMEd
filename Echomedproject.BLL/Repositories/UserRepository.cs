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
    internal class UserRepository : GenericRepository<Users>, IUsersRepository
    {

        public UserRepository(EckomedDbContext dbcontext) : base(dbcontext)
        {
        }

    }
}
