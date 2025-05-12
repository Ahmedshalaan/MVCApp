using Demo.DAL.Entities.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities.Departments
{
	// Department is a ModelBase
	public class Department : ModelBase
	{
		public string Name { get; set; } = null!;
		public string Code { get; set; } = null!;
		public string? Description { get; set; }
		public DateOnly CreationDate { get; set; }

		// Navigational Property [Many] [Will not Be Loaded => Related Data]
		public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
	}
}
