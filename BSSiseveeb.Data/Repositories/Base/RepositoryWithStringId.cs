using Sparkling.Data.Repositories;
using Sparkling.DataInterfaces;
using Sparkling.DataInterfaces.Domain;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BSSiseveeb.Data.Repositories.Base
{
    public class RepositoryWithStringId<T> : RepositoryWithTypedId<T, string>, IRepositoryWithTypedId<T, string>, IRepository, IQueryable<T>, IEnumerable<T>, IQueryable, IEnumerable where T : EntityWithTypedId<string>
    {

    }
}