using AutoMapper;
using EmployeeService.Models;
using EmployeeService.Models.Dto;
using EmployeeService.Services.Repositories;
using EmployeeServiceData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {

        #region Services 

        private readonly IEmployeeTypeRepository _employeeTypeRepository;
        private readonly ILogger<DictionariesController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public DictionariesController(
            IEmployeeTypeRepository employeeTypeRepository, 
            ILogger<DictionariesController> logger,
            IMapper mapper)
        {
            _employeeTypeRepository = employeeTypeRepository;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods


        [HttpPost("employee-types/create")]
        public ActionResult<int> CreateEmployeeType([FromQuery] string description)
        {
            return Ok(_employeeTypeRepository.Create(new EmployeeType
            {
                Description = description
            }));
        }

        [HttpPut("employee-types/update")]
        public ActionResult<bool> UpdateEmployeeType([FromQuery] int id, [FromQuery] string description)
        {
            return Ok(_employeeTypeRepository.Update(new EmployeeType { Id = id, Description = description }));
        }

        [HttpDelete("employee-types/delete")]
        public ActionResult<bool> DeleteEmployeeType([FromQuery] int id)
        {
            return Ok(_employeeTypeRepository.Delete(id));
        }

        [HttpGet("employee-types/all")]
        public ActionResult<IList<EmployeeTypeDto>> GetAllEmployeeTypes()
        {
            return Ok(_employeeTypeRepository.GetAll().Select(et =>
                _mapper.Map<EmployeeTypeDto>(et)
                ).ToList());
        }

        [HttpGet("employee-types/getbyid")]
        public ActionResult<EmployeeTypeDto> GetEmployeeTypeById([FromQuery] int id)
        {
            EmployeeType employeeType = _employeeTypeRepository.GetById(id);
            return  employeeType == null ? NotFound() :  Ok(_mapper.Map<EmployeeTypeDto>(employeeType));
        }

        #endregion

    }
}
