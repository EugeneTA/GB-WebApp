using EmployeeService.Models.Dto;
using EmployeeService.Models;
using EmployeeService.Services.Repositories;
using EmployeeService.Services.Repositories.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EmployeeServiceData;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        #region Services 

        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public DepartmentController(IDepartmentRepository departmentRepository, 
            ILogger<DepartmentController> logger,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods


        [HttpPost("create")]
        public ActionResult<Guid> CreateDepartment([FromQuery] string description)
        {
            return Ok(_departmentRepository.Create(new Department
            {
                Description = description
            }));
        }

        [HttpPut("update")]
        public ActionResult<bool> UpdateDepartment([FromQuery] Guid id, [FromQuery] string description)
        {
            return Ok(_departmentRepository.Update(new Department { Id = id, Description = description }));
        }

        [HttpDelete("delete")]
        public ActionResult<bool> DeleteDepartment([FromQuery] Guid id)
        {
            return Ok(_departmentRepository.Delete(id));
        }

        [HttpGet("getall")]
        public ActionResult<IList<DepartmentDto>> GetAllDepartments()
        {
            return Ok(_departmentRepository.GetAll().Select(dep => _mapper.Map<DepartmentDto>(dep)).ToList());
        }

        [HttpGet("getbyid")]
        public ActionResult<DepartmentDto> GetDepartmentById([FromQuery] Guid id)
        {
            Department department = _departmentRepository.GetById(id);
            return department == null ? NotFound() : Ok(_mapper.Map<DepartmentDto>(department));
        }

        #endregion
    }
}
