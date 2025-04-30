using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IAppUsersRepository
    {
        IEnumerable<AppUsers> GetAll();

        AppUsers Get(int id);

        int add (AppUsers user);

        int update (AppUsers user);
        int delete (AppUsers AppUsers);

    }
}
