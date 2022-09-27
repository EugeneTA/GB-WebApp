using EmployeeService.Models;

namespace EmployeeService.Services.Repositories.Impl
{
    public class EmployeeTypeRepository : IEmployeeTypeRepository
    {
        public int Create(EmployeeType data)
        {
            return 0;
            //throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            return true;
            //throw new NotImplementedException();
        }

        public IList<EmployeeType> GetAll()
        {
            return new List<EmployeeType>(new EmployeeType[] {
                new EmployeeType
                {
                    Id = 1,
                    Description = "Description #1"
                },
                new EmployeeType
                {
                    Id = 2,
                    Description = "Description #2"
                },
            });
            //throw new NotImplementedException();
        }

        public EmployeeType GetById(int id)
        {
            return new EmployeeType
            {
                Id = id,
                Description = "Description #1"
            };

            //throw new NotImplementedException();
        }

        public bool Update(EmployeeType data)
        {
            return true;
            //throw new NotImplementedException();
        }
    }
}
