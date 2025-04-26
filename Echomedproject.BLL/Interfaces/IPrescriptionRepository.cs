using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    internal interface IPrescriptionRepository
    {
        IEnumerable<prescription> GetAll();

        prescription Get(int id);

        int add(prescription prescription);

        int update(prescription prescription);
        int delete(prescription prescription);
    }
}
