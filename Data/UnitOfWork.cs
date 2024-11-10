using System;
using EMS.Models;
using EMS.Repositories;
namespace EMS.Data
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext appDbContext = new Data.ApplicationDbContext();

        private GenericRepository<Department> departmentRepository;
        private GenericRepository<Employee> employeeRepository;


        //Singleton parttern
        public GenericRepository<Department> DepartmentRepository
        {
            get
            {
                if(this.departmentRepository == null)
                {
                    this.departmentRepository = new GenericRepository<Department>(appDbContext);
                }
                return this.departmentRepository;
            }
        }

        //Singleton parttern
        public GenericRepository<Employee> EmployeeRepository
        {
            get
            {
                if (this.employeeRepository == null)
                {
                    this.employeeRepository = new GenericRepository<Employee>(appDbContext);
                }
                return this.employeeRepository;
            }
        }
      
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    appDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
