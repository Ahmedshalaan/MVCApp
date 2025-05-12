using Demo.DAL.Entities.Employees;
using Demo.DAL.Persistence.Repositories._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Persistence.Repositories.Employees
{
	public interface IEmployeeRepository : IGenericRepository<Employee>
	{
		
	}
}
