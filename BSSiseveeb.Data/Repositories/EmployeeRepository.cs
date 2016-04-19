using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using Sparkling.Data.Repositories;

namespace BSSiseveeb.Data.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
    }
}
