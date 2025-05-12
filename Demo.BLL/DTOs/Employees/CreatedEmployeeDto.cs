﻿using Demo.DAL.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.DTOs.Employees
{
	public class CreatedEmployeeDto
	{
		[Required(ErrorMessage = "Name Is Required")]
		[MaxLength(50, ErrorMessage = "Max Length Of Name Is 50 Chars")]
		[MinLength(3, ErrorMessage = "Min Length Of Name Is 3 Chars")]
		public string Name { get; set; } = null!;

		[Range(22, 30)]
		public int? Age { get; set; }
 
		public string? Address { get; set; }
		public decimal Salary { get; set; }

		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }

		[EmailAddress]

		public string? Email { get; set; }

		[Phone]
		[Display(Name = "Phone Number")]
		public string? PhoneNumber { get; set; }

		[Display(Name = "Hiring Date")]
		public DateOnly HiringDate { get; set; }
		public Gender Gender { get; set; }
		public EmployeeType EmployeeType {get; set;}

		[Display(Name = "Department")]
		public int? DepartmentId { get; set; }
		public IFormFile? Image { get; set; }

	}

}
