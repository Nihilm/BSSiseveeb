﻿using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using Sparkling.Data.Repositories;

namespace BSSiseveeb.Data.Repositories
{
    public class VacationRepository : Repository<Vacation>, IVacationRepository
    {
    }
}
