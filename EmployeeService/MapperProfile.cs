using AutoMapper;
using EmployeeService.Models;
using EmployeeService.Models.Dto;

namespace EmployeeService
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeType, EmployeeTypeDto>();

            CreateMap<DepartmentDto, Department>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<EmployeeTypeDto, EmployeeType>();

        }
    }
}
