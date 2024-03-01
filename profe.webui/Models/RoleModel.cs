using System;
using Microsoft.AspNetCore.Identity;
using profe.webui.Entities;
using System.ComponentModel.DataAnnotations;

namespace profe.webui.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class RoleDetails
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<User>? Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }

    }

    public class RoleEditModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[]? IdsToAdd { get; set; }
        public string[]? IdsToDelete { get; set; }
    }
}

