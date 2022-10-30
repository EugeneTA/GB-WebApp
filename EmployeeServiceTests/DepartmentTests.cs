using AutoMapper;
using EmployeeService.Controllers;
using EmployeeService.Models.Dto;
using EmployeeService.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeServiceTests
{
    public class DepartmentTests
    {
        private readonly DepartmentController _controller;
        private readonly Mock<IDepartmentRepository> _repository;
        private readonly Mock<ILogger<DepartmentController>> _logger;
        private readonly Mock<IMapper> _mapper;

        public DepartmentTests()
        {
            _repository = new Mock<IDepartmentRepository>();
            _logger = new Mock<ILogger<DepartmentController>>();
            _mapper = new Mock<IMapper>();
            _controller = new DepartmentController(_repository.Object, _logger.Object, _mapper.Object);
        }

        [Fact]
        public void GetAllDepartmentsTest()
        {
            _repository.Setup(rep => rep.GetAll()).Returns(new List<EmployeeServiceData.Department>());

            var result = _controller.GetAllDepartments();
            _repository.Verify(rep => rep.GetAll(), Times.Once);

            Assert.IsAssignableFrom<ActionResult<IList<DepartmentDto>>>(result);
        }


        [Theory]
        [InlineData("84691976-33EB-4322-9A64-319D36C4663E")]
        [InlineData("575C5898-07CA-48F4-9A3B-C5344854A0F9")]
        [InlineData("1EB9A852-56E3-40D6-8C2B-14D4FBC895DB")]
        public void GetDepartmentByIdTest([FromQuery] Guid id)
        {
            _repository.Setup(rep => rep.GetById(It.IsAny<Guid>())).Verifiable();
            var result = _controller.GetDepartmentById(id);
            _repository.Verify(rep => rep.GetById((It.IsAny<Guid>())), Times.Once);
            Assert.IsAssignableFrom<ActionResult<DepartmentDto>>(result);
        }

        [Theory]
        [InlineData("Department 1")]
        [InlineData("Department 2")]
        [InlineData("Department 3")]
        public void CreateDepartmentTest([FromQuery] string description)
        {
            _repository.Setup(rep => rep.Create(It.IsAny<EmployeeServiceData.Department>())).Verifiable();

            var result = _controller.CreateDepartment(description);

            _repository.Verify(rep => rep.Create(It.IsAny<EmployeeServiceData.Department>()), Times.Once());

            Assert.IsAssignableFrom<ActionResult<Guid>>(result);
        }

        [Theory]
        [InlineData("84691976-33EB-4322-9A64-319D36C4663E", "Department 22")]
        [InlineData("575C5898-07CA-48F4-9A3B-C5344854A0F9", "Department 23")]
        [InlineData("1EB9A852-56E3-40D6-8C2B-14D4FBC895DB", "Department 21")]
        public void UpdateDepartmentTest([FromQuery] Guid id, [FromQuery] string description)
        {
            _repository.Setup(rep => rep.Update(It.IsAny<EmployeeServiceData.Department>())).Verifiable();
            var result = _controller.UpdateDepartment(id, description);
            _repository.Verify(rep => rep.Update(It.IsAny<EmployeeServiceData.Department>()), Times.Once());
            Assert.IsAssignableFrom<ActionResult<bool>>(result);
        }

        [Theory]
        [InlineData("84691976-33EB-4322-9A64-319D36C4663E")]
        [InlineData("575C5898-07CA-48F4-9A3B-C5344854A0F9")]
        [InlineData("1EB9A852-56E3-40D6-8C2B-14D4FBC895DB")]
        public void DeleteDepartmentTest([FromQuery] Guid id)
        {
            _repository.Setup(rep => rep.Delete(It.IsAny<Guid>())).Verifiable();
            var result = _controller.DeleteDepartment(id);
            _repository.Verify(rep => rep.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.IsAssignableFrom<ActionResult<bool>>(result);
        }
    }
}
