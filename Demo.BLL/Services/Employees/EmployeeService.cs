using Demo.BLL.Common.Services.Attachments;
using Demo.BLL.DTOs.Employees;
using Demo.DAL.Entities.Employees;
using Demo.DAL.Persistence.Repositories.Employees;
using Demo.DAL.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Employees
{
	public class EmployeeService : IEmployeeService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAttachmentService _attachmentService;

		public EmployeeService(
			IUnitOfWork unitOfWork,
			IAttachmentService attachmentService
			) // Ask CLR For Creating Object from IUnitOfWork
		{
			_unitOfWork = unitOfWork;
			_attachmentService = attachmentService;
		}
		public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(string search)
		{
			var employees = await _unitOfWork.EmployeeRepository
						   .GetIQueryable()
						   .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(search) || E.Name.ToLower().Contains(search.ToLower())))
						   .Include(E => E.Department)
						   .Select(employee => new EmployeeDto()
						   {
							   Id = employee.Id,
							   Name = employee.Name,
							   Age = employee.Age,
							   IsActive = employee.IsActive,
							   Salary = employee.Salary,
							   Email = employee.Email,
							   Gender = employee.Gender.ToString(),
							   EmployeeType = employee.EmployeeType.ToString(),
							   Department = employee.Department.Name,
							   Image = employee.Image,
						   }).ToListAsync();


			return employees;
		}
		public async Task<EmployeeDetailsDto?> GetEmployeeByIdAsync(int id)
		{
			var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);

			if (employee is { })
				return new EmployeeDetailsDto()
				{
					Id = employee.Id,
					Name = employee.Name,
					Age = employee.Age,
					Address = employee.Address,
					IsActive = employee.IsActive,
					Salary = employee.Salary,
					Email = employee.Email,
					PhoneNumber = employee.PhoneNumber,
					HirringDate = employee.HirringDate,
					Gender = employee.Gender,
					EmployeeType = employee.EmployeeType,
					Department = employee.Department?.Name ?? "" ,// Once Accesss Department[Name] 
					Image = employee.Image,
				
				};

			return null;
		}
		public async Task<int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto)
		{

			var employee = new Employee()
			{
				Name = employeeDto.Name,
				Age = employeeDto.Age,
				Address = employeeDto.Address,
				IsActive = employeeDto.IsActive,
				Salary = employeeDto.Salary,
				Email = employeeDto.Email,
				PhoneNumber = employeeDto.PhoneNumber,
				HirringDate = employeeDto.HiringDate,
				Gender = employeeDto.Gender,
				EmployeeType = employeeDto.EmployeeType,
				DepartmentId = employeeDto.DepartmentId,
				CreatedBy = 1, // UserId
				LastModifiedBy = 1,
				LastModifiedOn = DateTime.UtcNow,

			};

			if (employeeDto.Image is not null)
				employee.Image = await _attachmentService.UploadAsync(employeeDto.Image, "images");

			_unitOfWork.EmployeeRepository.Add(employee);

			return await _unitOfWork.CompleteAsync();
		}
		public async Task<int> UpdateEmployeeasync(UpdatedEmployeeDto employeeDto)
		{
			var employee = new Employee()
			{
				Id = employeeDto.Id,
				Name = employeeDto.Name,
				Age = employeeDto.Age,
				Address = employeeDto.Address,
				IsActive = employeeDto.IsActive,
				Salary = employeeDto.Salary,
				Email = employeeDto.Email,
				PhoneNumber = employeeDto.PhoneNumber,
				HirringDate = employeeDto.HiringDate,
				Gender = employeeDto.Gender,
				EmployeeType = employeeDto.EmployeeType,
				DepartmentId = employeeDto.DepartmentId,
				CreatedBy = 1,
				LastModifiedBy = 1,
				LastModifiedOn = DateTime.UtcNow,
			};

			_unitOfWork.EmployeeRepository.Update(employee);
			return await _unitOfWork.CompleteAsync();
		}
		public async Task<bool> DeleteEmployeeAsync(int id)
		{
			var employeeRepo = _unitOfWork.EmployeeRepository;

			var employee = await employeeRepo.GetAsync(id);

			if (employee is { })
				employeeRepo.Delete(employee);

			return await _unitOfWork.CompleteAsync() > 0;
		}

	}
}
