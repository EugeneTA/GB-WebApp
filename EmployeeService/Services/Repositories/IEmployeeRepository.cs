using EmployeeService.Models;

namespace EmployeeService.Services.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee, int>
    {
    }
}
