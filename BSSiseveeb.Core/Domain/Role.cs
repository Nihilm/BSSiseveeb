using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BSSiseveeb.Core.Domain
{
    public class Role : IdentityRole
    {
       /* public Role()
        {
            Rights.HasFlag(AccessRights.Level1);
        }*/

        public AccessRights Rights { get; set; }
    }

    [Flags]
    public enum AccessRights
    {
        None = 0,
        Level1 = 1 << 0,
        Level2 = 1 << 1,
        Level3 = 1 << 2,
        Level4 = 1 << 3,
        Level5 = 1 << 4,
        All = Level1 | Level2 | Level3 | Level4 | Level5
    }
}
