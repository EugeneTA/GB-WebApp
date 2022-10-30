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
    public class DictionariesControllerTests
    {
        private readonly DictionariesController _dictionariesController;
        private readonly Mock<IEmployeeTypeRepository> _repository;
        private readonly Mock<ILogger<DictionariesController>> _logger;
        private readonly Mock<IMapper> _mapper;

        public DictionariesControllerTests()
        {
            _repository = new Mock<IEmployeeTypeRepository>();
            _logger = new Mock<ILogger<DictionariesController>>();
            _mapper = new Mock<IMapper>();
            _dictionariesController = new DictionariesController(_repository.Object, _logger.Object, _mapper.Object);
        }


        /*
         * [1] Подготовка данных
         * [2] Исполнение тестируемого метода
         * [3] Подготовка эталонного результата и проверка на валидность
         */

        [Theory]
        [InlineData("Programmer")]
        [InlineData("Manager")]
        [InlineData("CEO")]
        public void CreateEmployeeType([FromQuery] string description)
        {
            _repository.Setup(rep => rep.Create(It.IsAny<EmployeeServiceData.EmployeeType>())).Verifiable();

            var result = _dictionariesController.CreateEmployeeType(description);

            _repository.Verify(rep => rep.Create(It.IsAny<EmployeeServiceData.EmployeeType>()), Times.Once());

            Assert.IsAssignableFrom<ActionResult<int>>(result);
        }

        [Theory]
        [InlineData(1, "C# Programmmer")]
        [InlineData(2, "Accountant")]
        [InlineData(3, "CTO")]
        public void UpdateEmployeeType([FromQuery] int id, [FromQuery] string description)
        {
            _repository.Setup(rep => rep.Update(It.IsAny<EmployeeServiceData.EmployeeType>())).Verifiable();
            var result = _dictionariesController.UpdateEmployeeType(id, description);
            _repository.Verify(rep => rep.Update(It.IsAny<EmployeeServiceData.EmployeeType>()), Times.Once());
            Assert.IsAssignableFrom<ActionResult<bool>>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void DeleteEmployeeType([FromQuery] int id)
        {
            _repository.Setup(rep => rep.Delete(It.IsAny<int>())).Verifiable();
            var result = _dictionariesController.DeleteEmployeeType(id);
            _repository.Verify(rep => rep.Delete(It.IsAny<int>()), Times.Once());
            Assert.IsAssignableFrom<ActionResult<bool>>(result);
        }

        [Fact]
        public void GetAllEmployeeTypes()
        {
            _repository.Setup(rep => rep.GetAll()).Returns(new List<EmployeeServiceData.EmployeeType>());

            var result = _dictionariesController.GetAllEmployeeTypes();
            _repository.Verify(rep => rep.GetAll(), Times.Once);

           Assert.IsAssignableFrom<ActionResult<IList<EmployeeTypeDto>>>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetEmployeeTypeById([FromQuery] int id)
        {
            _repository.Setup(rep => rep.GetById(It.IsAny<int>())).Verifiable();
            var result = _dictionariesController.GetEmployeeTypeById(id);
            _repository.Verify(rep => rep.GetById((It.IsAny<int>())), Times.Once);
            Assert.IsAssignableFrom<ActionResult<EmployeeTypeDto>>(result);
        }
    }
}
