using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Departments
{
	public class DepartmentViewModel
	{
		[Required(ErrorMessage = "Code Is Required Ya Hamda !!")]
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? Description { get; set; }

		[Display(Name = "Creation Date")]
		public DateOnly CreationDate { get; set; }
	}
}
