using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using Sparkling.Data.Repositories;

namespace BSSiseveeb.Data.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
    }
}
