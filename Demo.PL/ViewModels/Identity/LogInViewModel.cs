using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Identity
{
	public class LogInViewModel
	{
		[EmailAddress]
		public string Email { get; set; } = null!;

		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
		public bool RememberMe { get; set; }
	}
}
