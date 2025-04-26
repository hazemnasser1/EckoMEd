using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Interfaces
{
    internal interface IInvoiceRepository
    {
        IEnumerable<Invoice> GetAll();

        Invoice Get(int id);

        int add(Invoice Invoice);

        int update(Invoice Invoice);
        int delete(Invoice Invoice);
    }
}
