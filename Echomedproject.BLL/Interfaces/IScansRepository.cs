using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IScansRepository
    {
        IEnumerable<Scans> GetAll();

        Scans Get(int id);

        int add(Scans Scans);

        int update(Scans Scans);
        int delete(Scans Scans);
    }
}
