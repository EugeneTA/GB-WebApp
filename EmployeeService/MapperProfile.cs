using AutoMapper;
using EmployeeService.Models;
using EmployeeService.Models.Dto;
using EmployeeService.Models.Requests;
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
            CreateMap<Employee, EmployeeCreateRequest>();
            CreateMap<EmployeeDto, EmployeeCreateRequest>();

            CreateMap<DepartmentDto, Department>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<EmployeeTypeDto, EmployeeType>();
            CreateMap<EmployeeCreateRequest, Employee>();
            CreateMap<EmployeeCreateRequest, EmployeeDto>();

        }
    }
}
