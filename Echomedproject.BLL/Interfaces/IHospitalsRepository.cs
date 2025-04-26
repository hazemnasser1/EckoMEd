using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    internal interface IHospitalsRepository
    {
        IEnumerable<Hospitals> GetAll();

        Hospitals Get(int id);

        int add(Hospitals hospital);

        int update(Hospitals hospitals);
        int delete(Hospitals Hospitals);
    }
}
