using EmployeeService.Models.Dto;
using EmployeeService.Models;
using EmployeeService.Services.Repositories;
using EmployeeService.Services.Repositories.Impl;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EmployeeServiceData;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using FluentValidation.Results;
using EmployeeService.Models.Requests;

namespace EmployeeService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        
        #region Services 

        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeCreateRequest> _employeeCreateRequestValidator;
        private readonly IValidator<EmployeeDto> _employeeValidator;

        #endregion

        #region Constructors

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            ILogger<EmployeeController> logger,
            IMapper mapper,
            IValidator<EmployeeCreateRequest> employeeCreateRequestValidator,
            IValidator<EmployeeDto> employeeValidator)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
            _employeeCreateRequestValidator = employeeCreateRequestValidator;
            _employeeValidator = employeeValidator;
        }

        #endregion

        #region Public Methods

        [HttpPost("employee-types/create")]
        public ActionResult<int> CreateEmployee([FromBody] EmployeeCreateRequest employee)
        {
            ValidationResult validationResult = _employeeCreateRequestValidator.Validate(employee);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ToDictionary());

            return Ok(_employeeRepository.Create(_mapper.Map<Employee>(employee)));
        }

        [HttpPut("employee-types/update")]
        public ActionResult<bool> UpdateEmployee([FromBody] EmployeeDto employee)
        {
            ValidationResult validationResult = _employeeValidator.Validate(employee);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.ToDictionary());

            return Ok(_employeeRepository.Update(_mapper.Map<Employee>(employee)));
        }

        [HttpDelete("employee-types/delete")]
        public ActionResult<bool> DeleteEmployee([FromQuery] int id)
        {
            return Ok(_employeeRepository.Delete(id));
        }

        [HttpGet("employee-types/all")]
        public ActionResult<IList<EmployeeDto>> GetAllEmployees()
        {
            return Ok(_employeeRepository.GetAll().ToList());
        }

        [HttpGet("employee-types/getbyid")]
        public ActionResult<EmployeeDto> GetEmployeeById([FromQuery] int id)
        {
            Employee employee = _employeeRepository.GetById(id);
            return employee == null ? NotFound() : Ok(_mapper.Map<EmployeeDto>(employee));
        }

        #endregion

    }
}
