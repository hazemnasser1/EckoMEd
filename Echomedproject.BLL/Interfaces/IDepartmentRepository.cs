using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    internal interface IDepartmentRepository
    {
        IEnumerable<Departments> GetAll();

        Departments Get(int id);

        int add(Departments Departments);

        int update(Departments Departments);
        int delete(Departments Departments);
    }
}
