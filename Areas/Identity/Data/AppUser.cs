﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AngularProject.Areas.Identity.Data;
public enum Gender { Female, Male }

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{

    public string ProfileImage { get; set; }
    public Gender Gender {get; set;}
}

