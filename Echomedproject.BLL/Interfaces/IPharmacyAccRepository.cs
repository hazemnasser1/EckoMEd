using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Migrations;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    public interface IPharmacyAccRepository: IGenericRepository<PharmacyAcc>
    {

        public PharmacyAcc? getpharmacyaccWithDetails(string pharmacyID);
    }
}
