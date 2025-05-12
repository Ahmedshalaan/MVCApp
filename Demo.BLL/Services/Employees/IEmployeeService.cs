using Demo.BLL.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Employees
{
	public interface IEmployeeService
	{
		Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(string search);
		Task<EmployeeDetailsDto?> GetEmployeeByIdAsync(int id);
		Task<int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto);
		Task<int> UpdateEmployeeasync(UpdatedEmployeeDto employeeDto);
		Task<bool> DeleteEmployeeAsync(int id);
	}
}
