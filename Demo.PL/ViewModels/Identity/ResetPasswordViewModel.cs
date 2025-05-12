using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Identity
{
	public class ResetPasswordViewModel
	{
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		[Display(Name = "Confirm Password")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "ConfirmPassword doesn't match Password")]
		public string ConfirmPassword { get; set; } = null!;
	}
}
