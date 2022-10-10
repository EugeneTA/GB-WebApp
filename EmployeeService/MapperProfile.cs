using AutoMapper;
using EmployeeService.Models;
using EmployeeService.Models.Dto;
using EmployeeServiceData;

namespace EmployeeService
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeType, EmployeeTypeDto>();
            CreateMap<Employee, EmployeeCreateDto>();
            CreateMap<EmployeeDto, EmployeeCreateDto>();

            CreateMap<DepartmentDto, Department>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<EmployeeTypeDto, EmployeeType>();
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeCreateDto, EmployeeDto>();

        }
    }
}
