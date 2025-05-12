using AutoMapper;
using Demo.BLL.DTOs.Departments;
using Demo.PL.ViewModels.Departments;

namespace Demo.PL.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			#region Department Module

			CreateMap<DepartmentDetailsDto, DepartmentViewModel>()
				/*.ForMember(dest => dest.Name, config => config.MapFrom(src => src.Name))*/;

			CreateMap<DepartmentViewModel, UpdatedDepartmentDto>();
			CreateMap<DepartmentViewModel, CreatedDepartmentDto>();

			#endregion

			#region Employee Module

			#endregion
		}
	}
}
