using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AngularProject;
public enum Gender { Female, Male }

// Add profile data for application users by adding properties to the AppUser class
public class User 
{
    [Key]
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }
    public string? ProfileImage { get; set; }
    public Gender Gender {get; set;}
}

