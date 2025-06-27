using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IPatientHospitalRepository : IGenericRepository<PatientHospital>
    {
        public PatientHospital GetPatientHospitalwithIDs(int hospitalID, string userID);
    }
}
