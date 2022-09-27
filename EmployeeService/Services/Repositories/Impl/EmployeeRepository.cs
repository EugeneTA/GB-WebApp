using EmployeeService.Models;

namespace EmployeeService.Services.Repositories.Impl
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public int Create(Employee data)
        {
            return data.Id;
            //throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            return true;
            //throw new NotImplementedException();
        }

        public IList<Employee> GetAll()
        {
            return new List<Employee>()
            {
                new Employee()
                {
                    Id = 0,
                    DepartmentId = Guid.NewGuid(),
                    EmployeeTypeId = 0,
                    FirstName = "Иванов",
                    Surname = "Иван",
                    Patronymic = "Иванович",
                    Salary = 100000
                },
                new Employee()
                {
                    Id = 1,
                    DepartmentId = Guid.NewGuid(),
                    EmployeeTypeId = 2,
                    FirstName = "Петров",
                    Surname = "Петр",
                    Patronymic = "петровия",
                    Salary = 200000
                }

            };
            //throw new NotImplementedException();
        }

        public Employee GetById(int id)
        {
            return new Employee()
            {
                Id = id,
                DepartmentId = Guid.NewGuid(),
                EmployeeTypeId = 0,
                FirstName = "Иванов",
                Surname = "Иван",
                Patronymic = "Иванович",
                Salary = 100000
            };

            //throw new NotImplementedException();
        }

        public bool Update(Employee data)
        {
            return true;
            //throw new NotImplementedException();
        }
    }
}
