using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AngularProject;
public enum Gender { Female, Male }

// Add profile data for application users by adding properties to the AppUser class
<<<<<<< HEAD
public class User :IdentityUser
{
=======
public class User : IdentityUser
{
    
    public string UserName { get; set; }
>>>>>>> 834f87c28afdc2c05114654d7e241947f39e6ead

    public string? ProfileImage { get; set; }
    public Gender Gender {get; set;}

    public string? Role { get; set; }
}

