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
        public List<PatientHospital> GetPatientsByDepartmentAndHospital(string departmentName, int hospitalID);
        public object GetGenderCountsByHospital(int hospitalID);
        public int GetTotalPatientsPreviousMonth(int hospitalID);
        public object GetLabTestCountsByMonth(int hospitalID);
        public object GetMonthlyPatientCounts(int hospitalID);

        public object GetMonthlyMalePatientCounts(int hospitalID);

        public object GetMonthlyFemalePatientCounts(int hospitalID);
        public object GetLabTestCountsByWeek(int hospitalID);
        public object GetWeeklyPatientCounts(int hospitalID);
        public object GetWeeklyMalePatientCounts(int hospitalID);
        public object GetWeeklyFemalePatientCounts(int hospitalID);

        public Dictionary<string, int> GetDailyPatientCountsForWeek(int hospitalID);

        public Dictionary<string, int> GetDailyPatientCountsForCurrentMonth(int hospitalID);
        public Dictionary<string, int> GetDailyPatientCountsForLastWeek(int hospitalID);

        public Dictionary<string, int> GetDailyPatientCountsForLastMonth(int hospitalID);




    }
}
