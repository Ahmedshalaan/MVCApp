using Demo.DAL.Persistence.Data;
using Demo.DAL.Persistence.Repositories.Departments;
using Demo.DAL.Persistence.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Persistence.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;

		public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_dbContext);
		public IDepartmentRepository DepartmentRepository => new  DepartmentRepository(_dbContext);

		public UnitOfWork(ApplicationDbContext dbContext) // ASK CLR For Creating Object From "ApplicationDbContext"
		{
			_dbContext = dbContext;
			
		}
		public async Task<int> CompleteAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}

		public async ValueTask DisposeAsync()
		{
			await _dbContext.DisposeAsync();
		}
	}
}
