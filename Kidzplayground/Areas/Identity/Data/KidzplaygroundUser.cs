using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Kidzplayground.Areas.Identity.Data;


public class KidzplaygroundUser : IdentityUser
{
  
    [PersonalData]
    [Display(Name = "Förnamn")]
    public string? FirstName { get; set; }

    [PersonalData]
    [Display(Name = "Efternamn")]
    public string? LastName { get; set; }

    [PersonalData]
    public int? BirthYear { get; set; }

    public string? ProfileImage { get; set; }

    public bool IsAdmin { get; set; }

}

