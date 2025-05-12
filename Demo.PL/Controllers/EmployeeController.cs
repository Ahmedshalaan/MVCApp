using Demo.BLL.DTOs.Employees;
using Demo.BLL.Services.Departments;
using Demo.BLL.Services.Employees;
using Demo.DAL.Entities.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
	// Inheritance : EmployeeController is Controller
	// Compostion  : EmployeeController has a EmployeeService
	[Authorize]
	public class EmployeeController : Controller
	{
		#region Services

		private readonly IEmployeeService _employeeService;
		//private readonly IDepartmentService _departmentService;
		private readonly ILogger<EmployeeController> _logger;
		private readonly IWebHostEnvironment _environment;

		public EmployeeController(
			IEmployeeService employeeService,
			//IDepartmentService departmentService,
			ILogger<EmployeeController> logger,
			IWebHostEnvironment environment
			)
		{
			_employeeService = employeeService;
			//_departmentService = departmentService;
			_logger = logger;
			_environment = environment;
		}

		#endregion

		#region Index

		[HttpGet] // GET: /Employee?Index
		public async Task<IActionResult> Index(string search)
		{
			var employees = await _employeeService.GetEmployeesAsync(search);

			return View(employees);
		}

		#endregion

		#region Create

		[HttpGet] // GET: /Employee/Create
		public IActionResult Create(/*[FromServices] IDepartmentService departmentService*/)
		{

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreatedEmployeeDto employee)
		{
			if (!ModelState.IsValid) // Server Side Validation
				return View(employee);

			var message = string.Empty;

			try
			{
				var result = await _employeeService.CreateEmployeeAsync(employee);

				if (result > 0)
					return RedirectToAction(nameof(Index));


				else
				{
					message = "Employee is not Created";
					ModelState.AddModelError(string.Empty, message);
					return View(employee);
				}
			}
			catch (Exception ex)
			{
				// 1. Log Exception
				_logger.LogError(ex, ex.Message);

				// 2. Set Message
				message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Creating Employee :(";


			}

			ModelState.AddModelError(string.Empty, message);

			return View(employee);
		}

		#endregion

		#region Details

		[HttpGet] // GET: Employee/Details/10
		public async Task<IActionResult> Details(int? id)
		{
			if (!id.HasValue)
				return BadRequest(); // 400

			var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

			if (employee is null)
				return NotFound(); // 404

			return View(employee);
		}

		#endregion

		#region Update

		[HttpGet] // GET: /Employee/Edit/id
		public async Task<IActionResult> Edit(int? id)
		{
			if (!id.HasValue)
				return BadRequest(); // 400	

			var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

			if (employee is null)
				return NotFound(); // 404



			return View(new UpdatedEmployeeDto()
			{
				Name = employee.Name,
				Address = employee.Address,
				Email = employee.Email,
				Age = employee.Age,
				Salary = employee.Salary,
				PhoneNumber = employee.PhoneNumber,
				IsActive = employee.IsActive,

				EmployeeType = employee.EmployeeType,
				Gender = employee.Gender,
				HiringDate = employee.HirringDate,
			});
		}

		[HttpPost] // POST: Employee/Edit/10
		public async Task<IActionResult> Edit([FromRoute] int id, UpdatedEmployeeDto emplpyee)
		{
			if (!ModelState.IsValid)
				return View(emplpyee);

			var message = string.Empty;

			try
			{

				var Updated = await _employeeService.UpdateEmployeeasync(emplpyee) > 0;

				if (Updated)
					return RedirectToAction(nameof(Index));

				message = "An Error Occured During Updating Employee :(";

			}
			catch (Exception ex)
			{
				// 1. Log Exception
				_logger.LogError(ex, ex.Message);

				// 2. Set Message

				message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Updating Employee :(";
			}

			ModelState.AddModelError(string.Empty, message);

			return View(emplpyee);
		}

		#endregion

		#region Delete

		[HttpGet] // GET: /Employee/Delete/id? 
		public async Task<IActionResult> Delete(int? id)
		{
			if (!id.HasValue)
				return BadRequest(); // 400

			var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

			if (employee is null)
				return NotFound(); // 404

			return View(employee);
		}

		[HttpPost] // POST: 
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var message = string.Empty;

			try
			{
				var deleted = await _employeeService.DeleteEmployeeAsync(id);

				if (deleted)
					return RedirectToAction(nameof(Index));

				message = "An Error Occured During Deleting This Employee :(";
			}
			catch (Exception ex)
			{

				// 1. Log Exception
				_logger.LogError(ex, ex.Message);

				// 2. Set Message

				message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Deleting Employee :(";
			}

			return RedirectToAction(nameof(Index));
		} 

		#endregion

	}
}
