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
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        private EckomedDbContext dbContext;

        public GenericRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public void add(T item)
        {
            dbContext.Set<T>().Add(item);
        }

        public void delete(T item)
        {
            dbContext.Set<T>().Remove(item);
        }

        public T Get(int id)
        {
            return dbContext.Set<T>().Find(id);
        }
         
        public IEnumerable<T> GetAll()
        {
            return dbContext.Set<T>().ToList();
        }

        public void update(T item)
        {
            dbContext.Set<T>().Update(item);
        }
    }
}
