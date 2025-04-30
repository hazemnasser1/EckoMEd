using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IPatientRepository
    {
        IEnumerable<Patients> GetAll();

        Patients Get(int id);

        int add(Patients Patients);

        int update(Patients Patients);
        int delete(Patients Patients);
    }
}
