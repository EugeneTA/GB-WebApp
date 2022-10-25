using AutoMapper;
using EmployeeService.Controllers;
using EmployeeService.Models.Dto;
using EmployeeService.Services.Repositories;
using EmployeeServiceProto;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using static EmployeeServiceProto.DictionariesService;

namespace EmployeeService.Services.gRPCServices
{
    public class DictionariesService : DictionariesServiceBase
    {
        #region Services 

        private readonly IEmployeeTypeRepository _employeeTypeRepository;
        private readonly ILogger<DictionariesController> _logger;

        public DictionariesService(IEmployeeTypeRepository employeeTypeRepository, ILogger<DictionariesController> logger)
        {
            _employeeTypeRepository = employeeTypeRepository;
            _logger = logger;
        }

        #endregion


        public override Task<CreateEmployeeTypeResponse> CreateEmployeeType(CreateEmployeeTypeRequest request, ServerCallContext context)
        {
            var id = _employeeTypeRepository.Create(new EmployeeServiceData.EmployeeType { Description = request.Description });

            return Task.FromResult(new CreateEmployeeTypeResponse { Id = id });
        }

        public override Task<UpdateEmployeeTypeResponse> UpdateEmployeeType(UpdateEmployeeTypeRequest request, ServerCallContext context)
        {
            var result = _employeeTypeRepository.Update(new EmployeeServiceData.EmployeeType { Id = request.EmployeeType.Id, Description = request.EmployeeType.Description });

            return Task.FromResult(new UpdateEmployeeTypeResponse { OperationResult = result });
        }

        public override Task<DeleteEmployeeTypeResponse> DeleteEmployeeType(DeleteEmployeeTypeRequest request, ServerCallContext context)
        {
            var result = _employeeTypeRepository.Delete(request.Id);

            return Task.FromResult(new DeleteEmployeeTypeResponse { OperationResult = result });
        }

        public override Task<GetAllEmployeeTypesResponse> GetAllEmployeeTypes(GetAllEmployeeTypesRequest request, ServerCallContext context)
        {
            GetAllEmployeeTypesResponse response = new GetAllEmployeeTypesResponse();

            response.EmployeeTypes.AddRange(_employeeTypeRepository.GetAll().Select(et =>
            new EmployeeType
            {
                Id = et.Id,
                Description = et.Description,
            }).ToList());

            return Task.FromResult(response);
        }


        public override Task<GetEmployeeTypeByIdResponse> GetEmployeeTypeById(GetEmployeeTypeByIdRequest request, ServerCallContext context)
        {
            GetEmployeeTypeByIdResponse getEmployeeTypeByIdResponse = new GetEmployeeTypeByIdResponse();

            var response = _employeeTypeRepository.GetById(request.Id);

            if (response != null)
            {
                getEmployeeTypeByIdResponse.EmployeeType.Id = response.Id;
                getEmployeeTypeByIdResponse.EmployeeType.Description = response.Description;
            }
            
            return Task.FromResult(getEmployeeTypeByIdResponse);
        }
    }
}
 