using DataAccess.Models;
using Infra.Wpf.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class AccountingUow : UnitOfWork
    {
        private IRepository<Person> personRepository;

        public IRepository<Person> PersonRepository
        {
            get { return personRepository ?? (personRepository = new PersonRepository(_context)); }
        }

        public AccountingUow() : this(new AccountingContext())
        {
        }
        public AccountingUow(System.Data.Entity.DbContext context) : base(context)
        {
            _context = context;
        }

        protected override void Dispose(bool disposing)
        {
            personRepository = null;

            base.Dispose(disposing);
        }
    }
}
