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
    internal class RoomRepository : IRoomRepository
    {
        private EckomedDbContext dbContext;

        public RoomRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(Room Room)
        {
           dbContext.rooms.Add(Room);
            return dbContext.SaveChanges();
        }

        public int delete(Room Room)
        {
            dbContext.rooms.Remove(Room);
            return dbContext.SaveChanges();
        }

        public Room Get(int id)
        {
            return dbContext.rooms.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<Room> GetAll()
        {
            return dbContext.rooms.ToList();
        }

        public int update(Room Room)
        {
            dbContext.rooms.Update(Room);
            return dbContext.SaveChanges();
        }
    }
}
