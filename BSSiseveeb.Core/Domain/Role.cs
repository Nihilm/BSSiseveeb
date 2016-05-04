using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BSSiseveeb.Core.Domain
{
    public class Role : IdentityRole
    {
        public AccessRights Rights { get; set; }
    }

    [Flags]
    public enum AccessRights
    {
        None = 0,
        Standard = 1 << 0,
        Requests = 1 << 1,
        Vacations = 1 << 2,
        Users = 1 << 3,
        Administrator = Standard | Requests | Vacations | Users
    }
}

