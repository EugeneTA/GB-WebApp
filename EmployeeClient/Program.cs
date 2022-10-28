using Grpc.Net.Client;
using static EmployeeServiceProto.DictionariesService;
using static EmployeeServiceProto.DepartmentService;
using static EmployeeServiceProto.EmployeeGrpcService;

namespace EmployeeClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var grpcCahnnel = GrpcChannel.ForAddress("https://localhost:5001");
            DictionariesServiceClient client = new DictionariesServiceClient(grpcCahnnel);
            DepartmentServiceClient departmentServiceClient = new DepartmentServiceClient(grpcCahnnel);
            EmployeeGrpcServiceClient employeeGrpcServiceClient = new EmployeeGrpcServiceClient(grpcCahnnel);

            //Console.WriteLine("Укажите тип сотрудника:");

            //var response = client.CreateEmployeeType(new EmployeeServiceProto.CreateEmployeeTypeRequest
            //{
            //    Description = Console.ReadLine()
            //});

            //if (response != null)
            //{
            //    Console.WriteLine($"Тип сотрудника добавлен успешно. ID: {response.Id}");
            //}
            //else
            //{
            //    Console.WriteLine("Ошибка при добавлении нового типа сотрудника");
            //}

            var allEmployeeTypesResponse = client.GetAllEmployeeTypes(new EmployeeServiceProto.GetAllEmployeeTypesRequest());
            Console.WriteLine("Специальности:");
            foreach (var employeeType in allEmployeeTypesResponse.EmployeeTypes)
            {
                Console.WriteLine($"{employeeType.Id} : {employeeType.Description}");
            }

            var allDepartmentsResponse = departmentServiceClient.GetAllDepartments(new EmployeeServiceProto.GetAllDepartmentsRequest());

            Console.WriteLine("\nДепартаменты:");
            foreach (var department in allDepartmentsResponse.DepartmentTypes)
            {
                Console.WriteLine($"{department.Id} : {department.Description}");
            }

            var allEmployeesResponse = employeeGrpcServiceClient.GetAllEmployees(new EmployeeServiceProto.GetAllEmployeesRequest());

            Console.WriteLine("\nСотрудники:");
            foreach (var employee in allEmployeesResponse.Employees)
            {
                Console.WriteLine($"{employee.Id} : {employee.DepartmentId}, {employee.EmployeeTypeId}, {employee.FirstName}, {employee.Surname}, {employee.Patronymic}, {(decimal)employee.Salary}");
            }

            Console.ReadKey();
        }
    }

}