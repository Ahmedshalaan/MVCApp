using AutoMapper;
using Demo.BLL.DTOs.Departments;
using Demo.BLL.Services.Departments;
using Demo.DAL.Entities.Departments;
using Demo.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
	// Inheritance : DepartmentController is Controller
	// Compostion  : DepartmentController has a DepartmentService
	[Authorize]
	public class DepartmentController : Controller
	{
		#region Services

		private readonly IDepartmentService _departmentService;
		private readonly ILogger<DepartmentController> _logger;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _environment;

		public DepartmentController(
			IDepartmentService departmentService,
			ILogger<DepartmentController> logger,
			IMapper mapper,
			IWebHostEnvironment environment
			)
		{
			_departmentService = departmentService;
			_logger = logger;
			_mapper = mapper;
			_environment = environment;
		}

		#endregion

		#region Index

		// View Storage : ViewData, ViewBag ==> Deal With The Storage
		// Dictionary
		// ExtraData
		// 1. Send Data from Action To View
		// 2. Send Data from View to PartialView
		// 3. Send Data from View To layout

		[HttpGet] // GET: /Department?Index
		public async Task<IActionResult> Index()
		{
			// View's Dictionary : Pass Data from Controller [Action] To View (from View --> [Partial View , Layout])

			// 1. ViewData : is a Dictionary Type Property (ASP.Net Framework 3.5)
			//              => Property is inehrited From Controller Class [Dictionary]
			//              => Its Helps To Transfer Data From Controller [Action] To View


			ViewData["Message"] = "Hello View Data";

			// 2. ViewBag : is a Dynamic Type Property (ASP.Net Framework 4.0)
			//              => Property is inehrited From Controller Class [Dictionary]
			//              => Its Helps To Transfer Data From Controller [Action] To View

			ViewBag.Message = "Hello View Bag";
			// ViewBag.Obj = new {Name = "Doaa", Id = 1 };

			var departments = await _departmentService.GetAllDepartmentsAsync();

			return View(departments);
		}

		#endregion

		#region Create

		[HttpGet] // GET: /Department/Create
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken] // Action Filter
		public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
		{
			if (!ModelState.IsValid) // Server Side Validation
				return View(departmentVM);

			var message = string.Empty;

			try
			{
				// Manual Mapping
				/// var CreatedDepartment = new CreatedDepartmentDto()
				/// {
				/// 	Code = departmentVM.Code,
				/// 	Name = departmentVM.Name,
				/// 	Description = departmentVM.Description,
				/// 	CreationDate = departmentVM.CreationDate,
				/// };

				var CreatedDepartment = _mapper.Map<CreatedDepartmentDto>(departmentVM);

				var Created = await _departmentService.CreateDepartmentAsync(CreatedDepartment) > 0;

				// 3. TempData : is a Dicatinary Type Property (ASP.Net Framework 3.5)
				//              => Property is inehrited From Controller Class [Dictionary]
				//              => Transfer Data From Request To Another Request [From Action To Another Action]

				if (Created)
				{
					TempData["Message"] = "Department is Created Successfuly";

					
				}


				else
				{
					message = "Department is not Created";
					TempData["Message"] = message;
					ModelState.AddModelError(string.Empty, message);
					// return View(departmentVM);
				}

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// 1. Log Exception
				_logger.LogError(ex, ex.Message);

				// 2. Set Message
				message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Creating Department :(";


			}

			ModelState.AddModelError(string.Empty, message);

			return View(departmentVM);
		}

		#endregion

		#region Details

		[HttpGet] // GET: Department/Details/10
		public async Task<IActionResult> Details(int? id)
		{
			if (!id.HasValue)
				return BadRequest(); // 400

			var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

			if (department is null)
				return NotFound(); // 404

			return View(department);
		}

		#endregion

		#region Update

		[HttpGet] // GET: /Department/Edit/id
		public async Task<IActionResult> Edit(int? id)
		{
			if (!id.HasValue)
				return BadRequest(); // 400	

			var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

			if (department is null)
				return NotFound(); // 404

			var departmentVM = _mapper.Map<DepartmentDetailsDto, DepartmentViewModel>(department);

			// Manual Mapping
			/// return View(new DepartmentViewModel()
			/// {
			/// 	Code = department.Code,
			/// 	Name = department.Name,
			/// 	Description = department.Description,
			/// 	CreationDate = department.CreationDate,
			/// });
			

			return View(departmentVM);
		}

		[HttpPost] // POST: Department/Edit/10
		[ValidateAntiForgeryToken] // Action Filter

		public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel departmentVM)
		{
			if (!ModelState.IsValid)
				return View(departmentVM);

			var message = string.Empty;

			try
			{
				// Manual Mapping
				/// var departmentToUpdate = new UpdatedDepartmentDto()
				/// {
				/// 	Id = id,
				/// 	Code = departmentVM.Code,
				/// 	Name = departmentVM.Name,
				/// 	Description = departmentVM.Description,
				/// 	CreationDate = departmentVM.CreationDate,
				/// };

				var departmentToUpdate = _mapper.Map<DepartmentViewModel, UpdatedDepartmentDto>(departmentVM);

				departmentToUpdate.Id = id;

				var Updated = await _departmentService.UpdateDepartmentAsync(departmentToUpdate) > 0;

				if (Updated)
					return RedirectToAction(nameof(Index));

				message = "An Error Occured During Updating Department :(";

			}
			catch (Exception ex)
			{
				// 1. Log Exception
				_logger.LogError(ex, ex.Message);

				// 2. Set Message

				message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Updating Department :(";
			}

			ModelState.AddModelError(string.Empty, message);

			return View(departmentVM);
		}

		#endregion

		#region Delete

		[HttpGet] // GET: /Department/Delete/id? 
		public async Task<IActionResult> Delete(int? id)
		{
			if (!id.HasValue)
				return BadRequest(); // 400

			var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

			if (department is null)
				return NotFound(); // 404

			return View(department);
		}

		[HttpPost] // POST: 
		[ValidateAntiForgeryToken] // Action Filter

		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var message = string.Empty;

			try
			{
				var deleted = await _departmentService.DeleteDepartmentAsync(id);

				if (deleted)
					return RedirectToAction(nameof(Index));

				message = "An Error Occured During Deleting This Department :(";
			}
			catch (Exception ex)
			{

				// 1. Log Exception
				_logger.LogError(ex, ex.Message);

				// 2. Set Message

				message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Deleting Department :(";
			}

			return RedirectToAction(nameof(Index));
		} 

		#endregion

	}
}
