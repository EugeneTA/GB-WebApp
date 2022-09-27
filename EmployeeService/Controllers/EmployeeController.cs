using EmployeeService.Models.Dto;
using EmployeeService.Models;
using EmployeeService.Services.Repositories;
using EmployeeService.Services.Repositories.Impl;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        
        #region Services 

        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            ILogger<EmployeeController> logger,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods


        [HttpPost("employee-types/create")]
        public ActionResult<int> CreateEmployee([FromBody] EmployeeDto employee)
        {
            return Ok(_employeeRepository.Create(_mapper.Map<Employee>(employee)));
        }

        [HttpPut("employee-types/update")]
        public ActionResult<bool> UpdateEmployee([FromBody] EmployeeDto employee)
        {
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
            return Ok(_employeeRepository.GetAll().Select(empl => _mapper.Map<EmployeeDto>(empl)).ToList());
        }

        [HttpGet("employee-types/getbyid")]
        public ActionResult<EmployeeDto> GetEmployeebyId([FromQuery] int id)
        {
            Employee employee = _employeeRepository.GetById(id);
            return employee == null ? NotFound() : Ok(_mapper.Map<EmployeeDto>(employee));
        }

        #endregion

    }
}
