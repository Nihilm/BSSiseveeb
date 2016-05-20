using BSSiseveeb.Core.Domain;
using System.Collections.Generic;
using System.IO;

namespace BSSiseveeb.Core.Contracts.Services
{
    public interface ICsvImportService : IApplicationService
    {
        List<Employee> parseEmployees(Stream stream);
    }
}
