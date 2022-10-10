using EmployeeService.Models;
using EmployeeServiceData;

namespace EmployeeService.Services.Repositories.Impl
{
    public class EmployeeTypeRepository : IEmployeeTypeRepository
    {
        private readonly EmployeeServiceDbContext _dbContext;

        public EmployeeTypeRepository(EmployeeServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Create(EmployeeType data)
        {
            if (data == null) return -1;
            _dbContext.EmployeeTypes.Add(data);
            _dbContext.SaveChanges();
            return data.Id;
        }

        public bool Delete(int id)
        {
            EmployeeType employeeType = GetById(id);
            if (employeeType == null) return false;
            _dbContext.EmployeeTypes.Remove(employeeType);
            return _dbContext.SaveChanges() > 0;
        }

        public IList<EmployeeType> GetAll()
        {
            return _dbContext.EmployeeTypes.ToList();
        }

        public EmployeeType GetById(int id)
        {
            return _dbContext.EmployeeTypes.FirstOrDefault(et => et.Id == id);
        }

        public bool Update(EmployeeType data)
        {
            if (data != null)
            {
                EmployeeType employeeType = GetById(data.Id);
                if (employeeType != null)
                {
                    employeeType.Description = data.Description;
                    return _dbContext.SaveChanges() > 0;
                }
            }
            
            return false;
        }
    }
}
