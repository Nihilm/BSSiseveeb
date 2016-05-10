using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Data.Repositories.Base;

namespace BSSiseveeb.Data.Repositories
{
    public class EmployeeRepository : RepositoryWithStringId<Employee>, IEmployeeRepository
    {

    }
}