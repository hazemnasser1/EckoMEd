using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    internal interface IRecordRepository
    {
        IEnumerable<Records> GetAll();

        Records Get(int id);

        int add(Records Records);

        int update(Records Records);
        int delete(Records Records);
    }
}
