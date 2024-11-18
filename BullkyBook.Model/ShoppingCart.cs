using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.Model
{
    public class ShoppingCart
    {

        [Key]
        public int Shop_id { get; set; }
       
        public int Pro_Id { get; set; }
        [ForeignKey("Pro_Id")]
        [ValidateNever]
        public ProductModel product { get; set; }
        [Range(1, 1000, ErrorMessage = "please enter a value Between 1 to 1000")]
        public int count { get; set; }

        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        [ValidateNever]
        public AppUser AppUser { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}
