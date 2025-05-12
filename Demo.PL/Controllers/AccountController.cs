using Demo.BLL.Common.Services.EmailSettings;
using Demo.DAL.Entities.Identity;
using Demo.PL.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
	public class AccountController : Controller
	{
		#region Services
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSetting _emailSetting;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSetting emailSetting
			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSetting = emailSetting;
		} 
		#endregion

		// SignUp(Register), SignIn(LogIn), SignOut ...

		#region Sign Up

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var User = await _userManager.FindByNameAsync(registerViewModel.UserName);

			if (User is { })
			{
				ModelState.AddModelError(nameof(registerViewModel.UserName), "This User Name is already Exist for Another Account");

				return View(registerViewModel);
			}

			User = new ApplicationUser()
			{
				// doaa@gmail.com
				FirstName = registerViewModel.FirstName,
				LastName = registerViewModel.LastName,
				UserName = registerViewModel.UserName,
				Email = registerViewModel.Email,
				IsAgree = registerViewModel.IsAgree,
			};

			var Result = await _userManager.CreateAsync(User, registerViewModel.Password);

			if (Result.Succeeded)
				return RedirectToAction("LogIn");

			foreach (var error in Result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View(registerViewModel); // ModelSate is no valid or Result no Succeeded
		}

		#endregion

		#region Sign In

		[HttpGet]
		public IActionResult LogIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LogIn(LogInViewModel logInViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var User = await _userManager.FindByEmailAsync(logInViewModel.Email); // Check User Exist with The Same Email

			if (User is { })
			{
				var flag = await _userManager.CheckPasswordAsync(User, logInViewModel.Password);

				if (flag) // email Exist & Password Correct
				{
					var Result = await _signInManager.PasswordSignInAsync(User, logInViewModel.Password, logInViewModel.RememberMe, false);

					if (Result.IsNotAllowed) // ConfirmedAccount
						ModelState.AddModelError(string.Empty, "Your Accoubt isn't Confirmed Yet!");

					if (Result.IsLockedOut)
						ModelState.AddModelError(string.Empty, "Your Accoubt Locked!");

					if (Result.Succeeded)
						return RedirectToAction((nameof(HomeController.Index)), "Home");

				}

			}

			ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

			return View(logInViewModel);
		}

		#endregion

		#region Sign Out

		[HttpGet]
		public async new Task<IActionResult> SignOut()
		{
			// Delete Token

			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(LogIn));
		}

		#endregion

		#region Forget Password

		[HttpGet]
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
		{

			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
				// Genetrate URL
				if(User is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(User);

					var resetPassword = Url.Action("ResetPassword", "Account", new { email = forgetPasswordViewModel.Email, token = token }, Request.Scheme);

					// Create Email
					// To, Subject, Body => DomainModel(Email) => {To, Subject, Body}

					var email = new Email()
					{
						To = forgetPasswordViewModel.Email,
						Subject = "Reset Your Password",
						Body = resetPassword ?? string.Empty
					};

					// Send Email
					_emailSetting.SendEmail(email);

					return RedirectToAction(nameof(CheckYourInbox));
				}

				ModelState.AddModelError(string.Empty, "Invalid Email");
				
			}

			return View(forgetPasswordViewModel);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}

		#endregion

		#region Reset Password

		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
		{
			if(ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;

				var User = await _userManager.FindByEmailAsync(email);

				if(User is not null)
				{
					var Result = await _userManager.ResetPasswordAsync(User, token, resetPasswordViewModel.Password);

					if (Result.Succeeded)
						return RedirectToAction(nameof(LogIn));

					foreach(var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
				
				}
			}

			ModelState.AddModelError(string.Empty, "Invalid Operation Please Try Again");

			return View(resetPasswordViewModel);
		}

		#endregion
	}
}
