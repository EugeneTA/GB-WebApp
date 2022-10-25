using EmployeeService.Controllers;
using EmployeeService.Services.Repositories;
using EmployeeServiceProto;
using Grpc.Core;
using static EmployeeServiceProto.EmployeeGrpcService;

namespace EmployeeService.Services.gRPCServices
{
    public class EmployeesService : EmployeeGrpcServiceBase
    {
        #region Services 

        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DictionariesController> _logger;

        public EmployeesService(IEmployeeRepository employeeRepository, ILogger<DictionariesController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        #endregion


        public override Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request, ServerCallContext context)
        {
            int id = 0;

            if (Guid.TryParse(request.DepartmentId, out var departmentId))
            {
                id = _employeeRepository.Create(new EmployeeServiceData.Employee 
                { 
                    DepartmentId = departmentId,
                    EmployeeTypeId = request.EmployeeTypeId,
                    FirstName = request.FirstName,
                    Surname = request.Surname,
                    Patronymic = request.Patronymic,
                    Salary = request.Salary
                });
            }

            return Task.FromResult(new CreateEmployeeResponse { Id = id });
        }

        public override Task<UpdateEmployeeResponse> UpdateEmployee(UpdateEmployeeRequest request, ServerCallContext context)
        {
            bool result = false;

            if (Guid.TryParse(request.Employee.DepartmentId, out var departmentId))
            {
                result = _employeeRepository.Update(new EmployeeServiceData.Employee
                {
                    Id = request.Employee.Id,
                    DepartmentId = departmentId,
                    EmployeeTypeId = request.Employee.EmployeeTypeId,
                    FirstName = request.Employee.FirstName,
                    Surname = request.Employee.Surname,
                    Patronymic = request.Employee.Patronymic,
                    Salary = request.Employee.Salary
                });
            }

            return Task.FromResult(new UpdateEmployeeResponse { OperationResult = result });
        }

        public override Task<DeleteEmployeeResponse> DeleteEmployee(DeleteEmployeeRequest request, ServerCallContext context)
        {
            var result = _employeeRepository.Delete(request.Id);

            return Task.FromResult(new DeleteEmployeeResponse { OperationResult = result });
        }

        public override Task<GetAllEmployeesResponse> GetAllEmployees(GetAllEmployeesRequest request, ServerCallContext context)
        {
            GetAllEmployeesResponse response = new GetAllEmployeesResponse();

            response.Employees.AddRange(_employeeRepository.GetAll().Select(et =>
            new Employee
            {
                Id = et.Id,
                DepartmentId = et.DepartmentId.ToString(),
                EmployeeTypeId = et.EmployeeTypeId,
                FirstName =et.FirstName,
                Surname =et.Surname,
                Patronymic =et.Patronymic,
                Salary =et.Salary
            }).ToList());

            return Task.FromResult(response);
        }


        public override Task<GetEmployeeByIdResponse> GetEmployeeById(GetEmployeeByIdRequest request, ServerCallContext context)
        {
            GetEmployeeByIdResponse getEmployeeByIdResponse = new GetEmployeeByIdResponse();

            var response = _employeeRepository.GetById(request.Id);

            if (response != null)
            {
                getEmployeeByIdResponse.Employee.Id = response.Id;
                getEmployeeByIdResponse.Employee.DepartmentId = response.DepartmentId.ToString();
                getEmployeeByIdResponse.Employee.EmployeeTypeId = response.EmployeeTypeId;
                getEmployeeByIdResponse.Employee.FirstName = response.FirstName;
                getEmployeeByIdResponse.Employee.Surname = response.Surname;
                getEmployeeByIdResponse.Employee.Patronymic = response.Patronymic;
                getEmployeeByIdResponse.Employee.Salary = response.Salary;
            }

            return Task.FromResult(getEmployeeByIdResponse);
        }
    }
}
