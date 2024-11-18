

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.Model
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string? User_name { get; set; }
        public string? StAddress { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? postalCode { get; set; }
        public int? Comp_Id { get; set; }
        [ForeignKey("Comp_Id")]
        [ValidateNever]

        public CompanyModel company { get; set; }




    }
}
