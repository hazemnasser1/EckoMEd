using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IDataEntryRepository
    {
        IEnumerable<DataEntry> GetAll();

        DataEntry Get(int id);

        int add(DataEntry DataEntry);

        int update(DataEntry DataEntry);
        int delete(DataEntry DataEntry);
    }
}
