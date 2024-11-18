using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BullkyBook.Model
{
    public class ProductModel
    {

        [Key]
        public int pro_id { get; set; }
        [Required]

        public string? pro_name { get; set; }
        [Required]
        [DisplayName("Product Description")]
        
        public string pro_description { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public double ListPrice { get; set; }

        [Required]
        public double price50 { get; set; }

        [Required]
        public double price100 { get; set; }
        [ValidateNever]
        public string ImageURL { get; set; }
        [Required]
        public int categoryID { get; set; }
        [ForeignKey("categoryID")]
        [ValidateNever]
        public BullkyBookModel category { get; set; }

        [Required]
        public int coverTypeID { get; set; }
        [ValidateNever]
        public CoverTypeModel coverType { get; set; }
    }
}
