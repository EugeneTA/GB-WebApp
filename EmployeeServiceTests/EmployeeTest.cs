using AutoMapper;
using EmployeeService;
using EmployeeService.Controllers;
using EmployeeService.Models.Dto;
using EmployeeService.Models.Requests;
using EmployeeService.Models.Validators;
using EmployeeService.Services.Repositories;
using FluentValidation;
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
    public class EmployeeTest
    {
        public static IEnumerable<object[]> DataCreate()
        {
            yield return new object[] { 
                new EmployeeCreateRequest
                {
                    DepartmentId = Guid.NewGuid(),
                    EmployeeTypeId = 1,
                    FirstName = "Name",
                    Surname = "Surname",
                    Patronymic = "Patronymic",
                    Salary = 100
                }};
        }

        public static IEnumerable<object[]> DataUpdate()
        {
            yield return new object[] {
                new EmployeeDto
                {
                    Id = 1,
                    DepartmentId = Guid.NewGuid(),
                    EmployeeTypeId = 1,
                    FirstName = "Name",
                    Surname = "Surname",
                    Patronymic = "Patronymic",
                    Salary = 100
                }};
        }

        private readonly EmployeeController _controller;
        private readonly Mock<IEmployeeRepository> _repository;
        private readonly Mock<ILogger<EmployeeController>> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeCreateRequest> _requesCreatetValidator;
        private readonly IValidator<EmployeeDto> _requestValidator;

        public EmployeeTest()
        {
            _repository = new Mock<IEmployeeRepository>();
            _logger = new Mock<ILogger<EmployeeController>>();

            // Create mapper configuration from our class MapperProfile
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));

            // Create mapper from our configuration
            _mapper = mapperConfiguration.CreateMapper();

            _requesCreatetValidator = new EmployeeCreateRequestValidator();
            _requestValidator = new EmployeeValidator();
            _controller = new EmployeeController(_repository.Object, _logger.Object, _mapper, _requesCreatetValidator, _requestValidator);
        }

        [Fact]
        public void GetAllEmployeesTest()
        {
            _repository.Setup(rep => rep.GetAll()).Returns(new List<EmployeeServiceData.Employee>());

            var result = _controller.GetAllEmployees();
            _repository.Verify(rep => rep.GetAll(), Times.Once);

            Assert.IsAssignableFrom<ActionResult<IList<EmployeeDto>>>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetEmployeebyIdTest([FromQuery] int id)
        {
            _repository.Setup(rep => rep.GetById(It.IsAny<int>())).Verifiable();
            var result = _controller.GetEmployeeById(id);
            _repository.Verify(rep => rep.GetById((It.IsAny<int>())), Times.Once);
            Assert.IsAssignableFrom<ActionResult<EmployeeDto>>(result);
        }


        [Theory]
        [MemberData(nameof(DataCreate))]
        public void CreateEmployeeTest([FromBody] EmployeeCreateRequest employee)
        {
            _repository.Setup(rep => rep.Create(It.IsAny<EmployeeServiceData.Employee>())).Verifiable();

            var result = _controller.CreateEmployee(employee);

            _repository.Verify(rep => rep.Create(It.IsAny<EmployeeServiceData.Employee>()), Times.Once());

            Assert.IsAssignableFrom<ActionResult<int>>(result);
        }

        [Theory]
        [MemberData(nameof(DataUpdate))]
        public void UpdateEmployeeTest([FromBody] EmployeeDto employee)
        {
            _repository.Setup(rep => rep.Update(It.IsAny<EmployeeServiceData.Employee>())).Verifiable();

            var result = _controller.UpdateEmployee(employee);

            _repository.Verify(rep => rep.Update(It.IsAny<EmployeeServiceData.Employee>()), Times.Once());

            Assert.IsAssignableFrom<ActionResult<bool>>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DeleteEmployeeTest([FromQuery] int id)
        {
            _repository.Setup(rep => rep.Delete(It.IsAny<int>())).Verifiable();
            var result = _controller.DeleteEmployee(id);
            _repository.Verify(rep => rep.Delete(It.IsAny<int>()), Times.Once());
            Assert.IsAssignableFrom<ActionResult<bool>>(result);
        }
    }
}
