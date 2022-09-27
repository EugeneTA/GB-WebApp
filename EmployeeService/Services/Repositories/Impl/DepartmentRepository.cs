using EmployeeService.Models;

namespace EmployeeService.Services.Repositories.Impl
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public Guid Create(Department data)
        {
            return Guid.NewGuid();
            //throw new NotImplementedException();
        }

       public IList<Department> GetAll()
        {
            return new List<Department>()
            {
                new Department()
                {
                    Id = Guid.NewGuid(),
                    Description = "IT"
                },
                new Department()
                {
                    Id = Guid.NewGuid(),
                    Description = "HR"
                }
            };
            //throw new NotImplementedException();
        }

        public Department GetById(Guid id)
        {
            return new Department()
            {
                Id = id,
                Description = "IT"
            };
        }

        public bool Update(Department data)
        {
            return true;
            //throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            return  true;
            //throw new NotImplementedException();
        }




    }
}
