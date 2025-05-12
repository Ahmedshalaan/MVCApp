using Demo.DAL.Entities.Departments;
using Demo.DAL.Persistence.Data;
using Demo.DAL.Persistence.Repositories._Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Persistence.Repositories.Departments
{
	public class DepartmentRepository : GenericRepository<Department> ,IDepartmentRepository
	{
		

		public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext) // Ask CLR For Creating Object From ApplicationDbContext
		{
			
		}

		
	}
}
