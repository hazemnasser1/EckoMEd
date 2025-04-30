using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface ILabTestRepository
    {
        IEnumerable<LabTest> GetAll();

        LabTest Get(int id);

        int add(LabTest LabTest);

        int update(LabTest LabTest);
        int delete(LabTest LabTest);
    }
}
