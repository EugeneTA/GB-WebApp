using EmployeeService.Models;
using EmployeeServiceData;

namespace EmployeeService.Services.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee, int>
    {
    }
}
