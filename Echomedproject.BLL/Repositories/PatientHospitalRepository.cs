using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Contexts;
using Echomedproject.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Echomedproject.BLL.Repositories
{
    public class PatientHospitalRepository :GenericRepository<PatientHospital>, IPatientHospitalRepository
    {
        EckomedDbContext dbContext;
        public PatientHospitalRepository(EckomedDbContext dbcontext) : base(dbcontext) {
        
        dbContext = dbcontext;
        }
        private (DateTime Start, DateTime End) GetWeekRange(int weeksAgo = 0)
        {
            var today = DateTime.Today;

            // DayOfWeek.Saturday = 6
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Saturday)) % 7;
            var startOfWeek = today.AddDays(-diff).Date.AddDays(-7 * weeksAgo);
            var endOfWeek = startOfWeek.AddDays(6);

            return (startOfWeek, endOfWeek);
        }


        public PatientHospital GetPatientHospitalwithIDs(int hospitalID, string userID)
        {
            return dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID
                          && ph.PatientId == userID
                          && ph.LeaveDate == null)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.LabTests)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.notes)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.Scans)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.prescription)
                        .ThenInclude(p => p.medicines) // assuming prescription has a Medicines list
                .Include(ph => ph.record)
                    .ThenInclude(r => r.Invoice)
                .OrderByDescending(ph => ph.EntryDate)
                .FirstOrDefault();
        }

        public List<PatientHospital> GetPatientsByDepartmentAndHospital(string departmentName, int hospitalID)
        {
            return dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID
                          && ph.Department == departmentName
                          && ph.LeaveDate == null)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.LabTests)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.notes)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.Scans)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.prescription)
                        .ThenInclude(p => p.medicines)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.Invoice)
                .OrderByDescending(ph => ph.EntryDate)
                .ToList();
        }

        public object GetGenderCountsByHospital(int hospitalID)
        {
            var genderList = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID)
                .Select(ph => ph.Gender)
                .ToList();

            int maleCount = genderList.Count(g => g != null && g.Equals("Male", StringComparison.OrdinalIgnoreCase));
            int femaleCount = genderList.Count(g => g != null && g.Equals("Female", StringComparison.OrdinalIgnoreCase));

            return new
            {
                Male = maleCount,
                Female = femaleCount
            };
        }

        public int GetTotalPatientsPreviousMonth(int hospitalID)
        {
            var today = DateTime.Today;
            var firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfLastMonth = firstDayOfThisMonth.AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfThisMonth.AddDays(-1);

            var count = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.EntryDate >= firstDayOfLastMonth &&
                             ph.EntryDate <= lastDayOfLastMonth)
                .Count();

            return count;
        }

        public object GetLabTestCountsByMonth(int hospitalID)
        {
            var today = DateTime.Today;
            var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

            var labTests = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID && ph.record != null)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.LabTests)
                .SelectMany(ph => ph.record.LabTests)
                .ToList();

            int currentMonthCount = labTests.Count(l =>
                l.Date.Month == firstDayOfCurrentMonth.Month &&
                l.Date.Year == firstDayOfCurrentMonth.Year
            );

            int lastMonthCount = labTests.Count(l =>
                l.Date >= firstDayOfLastMonth &&
                l.Date <= lastDayOfLastMonth
            );

            return new
            {
                CurrentMonthLabTests = currentMonthCount,
                LastMonthLabTests = lastMonthCount
            };
        }

        public object GetMonthlyPatientCounts(int hospitalID)
        {
            var today = DateTime.Today;

            var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

            var currentMonthCount = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.EntryDate >= firstDayOfCurrentMonth)
                .Count();

            var lastMonthCount = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.EntryDate >= firstDayOfLastMonth &&
                             ph.EntryDate <= lastDayOfLastMonth)
                .Count();

            return new
            {
                CurrentMonthPatients = currentMonthCount,
                LastMonthPatients = lastMonthCount
            };
        }

        public object GetMonthlyMalePatientCounts(int hospitalID)
        {
            var today = DateTime.Today;

            var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

            var currentMonthMales = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.Gender != null &&
                             ph.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase) &&
                             ph.EntryDate >= firstDayOfCurrentMonth)
                .Count();

            var lastMonthMales = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.Gender != null &&
                             ph.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase) &&
                             ph.EntryDate >= firstDayOfLastMonth &&
                             ph.EntryDate <= lastDayOfLastMonth)
                .Count();

            return new
            {
                CurrentMonthMales = currentMonthMales,
                LastMonthMales = lastMonthMales
            };
        }

        public object GetMonthlyFemalePatientCounts(int hospitalID)
        {
            var today = DateTime.Today;

            var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);
            var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

            var currentMonthFemales = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.Gender != null &&
                             ph.Gender.Equals("Female", StringComparison.OrdinalIgnoreCase) &&
                             ph.EntryDate >= firstDayOfCurrentMonth)
                .Count();

            var lastMonthFemales = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.Gender != null &&
                             ph.Gender.Equals("Female", StringComparison.OrdinalIgnoreCase) &&
                             ph.EntryDate >= firstDayOfLastMonth &&
                             ph.EntryDate <= lastDayOfLastMonth)
                .Count();

            return new
            {
                CurrentMonthFemales = currentMonthFemales,
                LastMonthFemales = lastMonthFemales
            };
        }

        public object GetLabTestCountsByWeek(int hospitalID)
        {
            var thisWeek = GetWeekRange(0);
            var lastWeek = GetWeekRange(1);

            var labTests = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID && ph.record != null)
                .Include(ph => ph.record)
                    .ThenInclude(r => r.LabTests)
                .SelectMany(ph => ph.record.LabTests)
                .ToList();

            int currentWeekCount = labTests.Count(l => l.Date >= thisWeek.Start && l.Date <= thisWeek.End);
            int lastWeekCount = labTests.Count(l => l.Date >= lastWeek.Start && l.Date <= lastWeek.End);

            return new
            {
                CurrentWeekLabTests = currentWeekCount,
                LastWeekLabTests = lastWeekCount
            };
        }

        public object GetWeeklyPatientCounts(int hospitalID)
        {
            var thisWeek = GetWeekRange(0);
            var lastWeek = GetWeekRange(1);

            var currentWeekCount = dbContext.patientHospital
                .Count(ph => ph.HospitalId == hospitalID && ph.EntryDate >= thisWeek.Start && ph.EntryDate <= thisWeek.End);

            var lastWeekCount = dbContext.patientHospital
                .Count(ph => ph.HospitalId == hospitalID && ph.EntryDate >= lastWeek.Start && ph.EntryDate <= lastWeek.End);

            return new
            {
                CurrentWeekPatients = currentWeekCount,
                LastWeekPatients = lastWeekCount
            };
        }

        public object GetWeeklyMalePatientCounts(int hospitalID)
        {
            var thisWeek = GetWeekRange(0);
            var lastWeek = GetWeekRange(1);

            int currentWeekMales = dbContext.patientHospital
                .Count(ph => ph.HospitalId == hospitalID &&
                             ph.Gender != null &&
                             ph.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase) &&
                             ph.EntryDate >= thisWeek.Start && ph.EntryDate <= thisWeek.End);

            int lastWeekMales = dbContext.patientHospital
                .Count(ph => ph.HospitalId == hospitalID &&
                             ph.Gender != null &&
                             ph.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase) &&
                             ph.EntryDate >= lastWeek.Start && ph.EntryDate <= lastWeek.End);

            return new
            {
                CurrentWeekMales = currentWeekMales,
                LastWeekMales = lastWeekMales
            };
        }

        public object GetWeeklyFemalePatientCounts(int hospitalID)
        {
            var thisWeek = GetWeekRange(0);
            var lastWeek = GetWeekRange(1);

            int currentWeekFemales = dbContext.patientHospital
                .Count(ph => ph.HospitalId == hospitalID &&
                             ph.Gender != null &&
                             ph.Gender.Equals("Female", StringComparison.OrdinalIgnoreCase) &&
                             ph.EntryDate >= thisWeek.Start && ph.EntryDate <= thisWeek.End);

            int lastWeekFemales = dbContext.patientHospital
                .Count(ph => ph.HospitalId == hospitalID &&
                             ph.Gender != null &&
                             ph.Gender.Equals("Female", StringComparison.OrdinalIgnoreCase) &&
                             ph.EntryDate >= lastWeek.Start && ph.EntryDate <= lastWeek.End);

            return new
            {
                CurrentWeekFemales = currentWeekFemales,
                LastWeekFemales = lastWeekFemales
            };
        }

        public Dictionary<string, int> GetDailyPatientCountsForWeek(int hospitalID)
        {
            // Get start and end of current week (Saturday to Friday)
            var (startOfWeek, endOfWeek) = GetWeekRange(0);

            var entries = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.EntryDate >= startOfWeek && ph.EntryDate <= endOfWeek)
                .ToList();

            // Initialize dictionary with all days of the week (starting Saturday)
            var dayCounts = new Dictionary<DayOfWeek, int>
    {
        { DayOfWeek.Saturday, 0 },
        { DayOfWeek.Sunday, 0 },
        { DayOfWeek.Monday, 0 },
        { DayOfWeek.Tuesday, 0 },
        { DayOfWeek.Wednesday, 0 },
        { DayOfWeek.Thursday, 0 },
        { DayOfWeek.Friday, 0 }
    };

            // Count patients per day
            foreach (var entry in entries)
            {
                var day = entry.EntryDate.DayOfWeek;
                if (dayCounts.ContainsKey(day))
                    dayCounts[day]++;
            }

            // Convert keys to Arabic-style or custom labels if needed (optional)
            var formatted = dayCounts
                .OrderBy(d => ((int)d.Key + 1) % 7) // Start from Saturday
                .ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);

            return formatted;
        }


        public Dictionary<string, int> GetDailyPatientCountsForCurrentMonth(int hospitalID)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // Last day of current month

            var entries = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.EntryDate >= startOfMonth && ph.EntryDate <= endOfMonth)
                .ToList();

            // Initialize dictionary with all days of the current month
            var dayCounts = new Dictionary<string, int>();
            for (int day = 1; day <= endOfMonth.Day; day++)
            {
                var date = new DateTime(today.Year, today.Month, day);
                dayCounts[date.ToString("yyyy-MM-dd")] = 0;
            }

            // Count patients per exact date
            foreach (var entry in entries)
            {
                var dateKey = entry.EntryDate.Date.ToString("yyyy-MM-dd");
                if (dayCounts.ContainsKey(dateKey))
                    dayCounts[dateKey]++;
            }

            return dayCounts;
        }

        public Dictionary<string, int> GetDailyPatientCountsForLastWeek(int hospitalID)
        {
            var (startOfWeek, endOfWeek) = GetWeekRange(1); // Last week

            var entries = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.EntryDate >= startOfWeek &&
                             ph.EntryDate <= endOfWeek)
                .ToList();

            var dayCounts = new Dictionary<string, int>();
            for (int i = 0; i < 7; i++)
            {
                var date = startOfWeek.AddDays(i);
                dayCounts[date.ToString("yyyy-MM-dd")] = 0;
            }

            foreach (var entry in entries)
            {
                var dateKey = entry.EntryDate.Date.ToString("yyyy-MM-dd");
                if (dayCounts.ContainsKey(dateKey))
                    dayCounts[dateKey]++;
            }

            return dayCounts;
        }

        public Dictionary<string, int> GetDailyPatientCountsForLastMonth(int hospitalID)
        {
            var today = DateTime.Today;
            var firstDayLastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            var lastDayLastMonth = new DateTime(today.Year, today.Month, 1).AddDays(-1);

            var entries = dbContext.patientHospital
                .Where(ph => ph.HospitalId == hospitalID &&
                             ph.EntryDate >= firstDayLastMonth &&
                             ph.EntryDate <= lastDayLastMonth)
                .ToList();

            var dayCounts = new Dictionary<string, int>();
            for (int day = 1; day <= lastDayLastMonth.Day; day++)
            {
                var date = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month, day);
                dayCounts[date.ToString("yyyy-MM-dd")] = 0;
            }

            foreach (var entry in entries)
            {
                var dateKey = entry.EntryDate.Date.ToString("yyyy-MM-dd");
                if (dayCounts.ContainsKey(dateKey))
                    dayCounts[dateKey]++;
            }

            return dayCounts;
        }

    }
}
