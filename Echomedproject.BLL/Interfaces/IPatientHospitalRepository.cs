using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    internal interface IPatientHospitalRepository
    {
        IEnumerable<PatientHospital> GetAll();

        PatientHospital Get(int id);

        int add(PatientHospital PatientHospital);

        int update(PatientHospital PatientHospital);
        int delete(PatientHospital PatientHospital);
    }
}
