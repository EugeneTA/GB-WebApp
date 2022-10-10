using EmployeeService.Models;
using EmployeeService.Models.Dto;
using EmployeeServiceData;

namespace EmployeeService.Services.Repositories.Impl
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeServiceDbContext _dbContext;

        public EmployeeRepository(EmployeeServiceDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public int Create(Employee data)
        {
            if (data == null) return -1;
            //_dbContext.Employees.Add(new Employee() { 
            //DepartmentId = data.DepartmentId,
            //EmployeeTypeId = data.EmployeeTypeId,
            //FirstName = data.FirstName,
            //Surname = data.Surname,
            //Patronymic  = data.Patronymic,
            //Salary = data.Salary
            //});
            _dbContext.Employees.Add(data);
            _dbContext.SaveChanges();
            return data.Id;
        }

        public IList<Employee> GetAll()
        {
            return _dbContext.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return _dbContext.Employees.FirstOrDefault(empl => empl.Id == id);
        }

        public bool Update(Employee data)
        {
            if (data != null)
            {
                Employee employee = GetById(data.Id);
                if (employee != null)
                {
                    employee.DepartmentId = employee.DepartmentId == data.DepartmentId ? employee.DepartmentId : data.DepartmentId;
                    employee.EmployeeTypeId = employee.EmployeeTypeId == data.EmployeeTypeId ? employee.EmployeeTypeId : data.EmployeeTypeId;
                    employee.FirstName = employee.FirstName == data.FirstName ? employee.FirstName : data.FirstName;
                    employee.Surname = employee.Surname == data.Surname ? employee.Surname : data.Surname;
                    employee.Patronymic = employee.Patronymic == data.Patronymic ? employee.Patronymic : data.Patronymic;
                    employee.Salary = employee.Salary == data.Salary ? employee.Salary : data.Salary;

                    return _dbContext.SaveChanges() > 0;
                }
            }

            return false;
        }

        public bool Delete(int id)
        {
            Employee employee = GetById(id);
            if (employee == null) return false;
            _dbContext.Employees.Remove(employee);
            return _dbContext.SaveChanges() > 0;
        }
    }
}
