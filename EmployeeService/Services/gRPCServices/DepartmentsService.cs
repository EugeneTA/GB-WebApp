using EmployeeService.Controllers;
using EmployeeService.Services.Repositories;
using EmployeeServiceProto;
using Grpc.Core;
using static EmployeeServiceProto.DepartmentService;

namespace EmployeeService.Services.gRPCServices
{
    public class DepartmentsService : DepartmentServiceBase
    {
        #region Services 

        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DictionariesController> _logger;

        public DepartmentsService(IDepartmentRepository departmentRepository, ILogger<DictionariesController> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        #endregion


        public override Task<CreateDepartmentResponse> CreateDepartment(CreateDepartmentRequest request, ServerCallContext context)
        {
            var id = _departmentRepository.Create(new EmployeeServiceData.Department { Description = request.Description });

            return Task.FromResult(new CreateDepartmentResponse { Id = id.ToString() });
        }

        public override Task<UpdateDepartmentResponse> UpdateDepartment(UpdateDepartmentRequest request, ServerCallContext context)
        {
            bool result = false;

            if (Guid.TryParse(request.DepartmentType.Id, out Guid departmentId))
            {
                result = _departmentRepository.Update(new EmployeeServiceData.Department { Id = departmentId, Description = request.DepartmentType.Description });
            }

            return Task.FromResult(new UpdateDepartmentResponse { OperationResult = result });
        }

        public override Task<DeleteDepartmentResponse> DeleteDepartment(DeleteDepartmentRequest request, ServerCallContext context)
        {
            bool result = false;

            if (Guid.TryParse(request.Id, out Guid departmentId))
            {
                result = _departmentRepository.Delete(departmentId);
            }

            return Task.FromResult(new DeleteDepartmentResponse { OperationResult = result });
        }

        public override Task<GetAllDepartmentsResponse> GetAllDepartments(GetAllDepartmentsRequest request, ServerCallContext context)
        {
            GetAllDepartmentsResponse response = new GetAllDepartmentsResponse();

            response.DepartmentTypes.AddRange(_departmentRepository.GetAll().Select(et =>
            new DepartmentType
            {
                Id = et.Id.ToString(),
                Description = et.Description,
            }).ToList());

            return Task.FromResult(response);
        }


        public override Task<GetDepartmentByIdResponse> GetDepartmentById(GetDepartmentByIdRequest request, ServerCallContext context)
        {
            GetDepartmentByIdResponse getDepartmentByIdResponse = new GetDepartmentByIdResponse();

            if (Guid.TryParse(request.Id, out Guid departmentId))
            {
                var response = _departmentRepository.GetById(departmentId);

                if (response != null)
                {
                    getDepartmentByIdResponse.DepartmentType.Id = response.Id.ToString();
                    getDepartmentByIdResponse.DepartmentType.Description = response.Description;
                }
            }

            return Task.FromResult(getDepartmentByIdResponse);
        }
    }
}
