using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    internal interface IRoomRepository
    {
        IEnumerable<Room> GetAll();

        Room Get(int id);

        int add(Room Room);

        int update(Room Room);
        int delete(Room Room);
    }
}
