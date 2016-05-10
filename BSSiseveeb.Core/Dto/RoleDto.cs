using BSSiseveeb.Core.Domain;


namespace BSSiseveeb.Core.Dto
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AccessRights Rights { get; set; }
    }
}
