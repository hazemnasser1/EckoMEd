using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echomedproject.BLL.Interfaces;
using Echomedproject.DAL.Contexts;
using Echomedproject.DAL.Models;

namespace Echomedproject.BLL.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private EckomedDbContext dbContext;

        public InvoiceRepository(EckomedDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public int add(Invoice Invoice)
        {
            dbContext.invoices.Add(Invoice);
            return dbContext.SaveChanges();
        }

        public int delete(Invoice Invoice)
        {
            dbContext.invoices.Remove(Invoice);
            return dbContext.SaveChanges();
        }

        public Invoice Get(int id)
        {
            return dbContext.invoices.Where(d => d.Id == id).FirstOrDefault();
        }

        public IEnumerable<Invoice> GetAll()
        {
            return dbContext.invoices.ToList();
        }

        public int update(Invoice Invoice)
        {
           dbContext.invoices.Update(Invoice);
            return dbContext.SaveChanges();
        }
    }
}
