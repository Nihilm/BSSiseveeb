﻿using BSSiseveeb.Core.Domain;
using Sparkling.DataInterfaces;

namespace BSSiseveeb.Core.Contracts.Repositories
{
    public interface IEmployeeRepository : IRepositoryWithTypedId<Employee, string>
    {

    }
}