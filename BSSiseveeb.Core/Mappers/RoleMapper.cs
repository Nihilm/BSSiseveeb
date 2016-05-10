using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BSSiseveeb.Core.Mappers
{
    public static class RoleMapper
    {
        public static RoleDto AsDto(this Role role)
        {
            return new RoleDto()
            {
                Id = role.Id,
                Name = role.Name,
                Rights = role.Rights
            };
        }

        public static IList<RoleDto> AsDto(this IQueryable<Role> query)
        {
            return query.Select(AsDto()).ToList();
        }

        public static Expression<Func<Role, RoleDto>> AsDto()
        {
            return (role) => new RoleDto()
            {
                Id = role.Id,
                Name = role.Name,
                Rights = role.Rights
            };
        }
    }
}
