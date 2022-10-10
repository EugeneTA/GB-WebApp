using EmployeeService.Models;
using EmployeeServiceData;

namespace EmployeeService.Services.Repositories.Impl
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EmployeeServiceDbContext _dbContext;

        public DepartmentRepository(EmployeeServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Create(Department data)
        {
            if (data == null) return Guid.Empty;
            _dbContext.Departments.Add(data);
            _dbContext.SaveChanges();
            return data.Id;
        }

       public IList<Department> GetAll()
        {
            return _dbContext.Departments.ToList();
        }

        public Department GetById(Guid id)
        {
            return _dbContext.Departments.FirstOrDefault(dp => dp.Id == id);
        }

        public bool Update(Department data)
        {
            if (data != null)
            {
                Department department = GetById(data.Id);
                if (department != null)
                {
                    department.Description = data.Description;
                    return _dbContext.SaveChanges() > 0;
                }
            }

            return false;
        }

        public bool Delete(Guid id)
        {
            Department department = GetById(id);
            if (department == null) return false;
            _dbContext.Departments.Remove(department);
            return _dbContext.SaveChanges() > 0;
        }

    }
}
