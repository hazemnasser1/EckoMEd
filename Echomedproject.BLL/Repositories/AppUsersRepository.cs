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
    internal class AppUsersRepository : IAppUsersRepository
    {
        private EckomedDbContext dbContext;

        public AppUsersRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(AppUsers user)
        {
            dbContext.Users.Add(user);
            return dbContext.SaveChanges();
        }

        public int delete(AppUsers AppUsers)
        {
            
            dbContext.Users.Remove(AppUsers);
            return dbContext.SaveChanges(); 
        }

        public AppUsers Get(int id)
        {

            return dbContext.Users.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<AppUsers> GetAll()
        {
            return dbContext.Users.ToList();
        }

        public int update(AppUsers user)
        {
            dbContext.Users.Update(user);
            return dbContext.SaveChanges();
        }
    }
}
