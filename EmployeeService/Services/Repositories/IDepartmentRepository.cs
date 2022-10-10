using EmployeeService.Models;
using EmployeeServiceData;

namespace EmployeeService.Services.Repositories
{
    public interface IDepartmentRepository : IRepository<Department, Guid>
    {
    }
}
