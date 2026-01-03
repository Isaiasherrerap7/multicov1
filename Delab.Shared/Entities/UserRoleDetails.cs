using Delab.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Shared.Entities;

public class UserRoleDetails
{
    [Key]
    public int UserRoleDetailsId { get; set; }

    [Display(Name = "Rol Usuario")]
    public UserType? UserType { get; set; }

    [Display(Name = "User Id")]
    public string? UserId { get; set; }

    //Relacion
    public User? User { get; set; }
}