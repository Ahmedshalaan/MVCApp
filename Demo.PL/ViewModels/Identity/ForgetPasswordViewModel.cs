using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Identity
{
	public class ForgetPasswordViewModel
	{
		[EmailAddress]
		public string Email { get; set; } = null!;
	}
}
