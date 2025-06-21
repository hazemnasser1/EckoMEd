using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IAppUsersRepository:IGenericRepository<AppUsers>
    {
        public AppUsers getUserbyEmail(string email);

        public AppUsers? getUserWithRecordDetails(string email);

        public Records? GetRecord(int Id);
       
      
    }
}
