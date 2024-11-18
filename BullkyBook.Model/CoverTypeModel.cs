using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.Model
{
    public class CoverTypeModel
    {

        [Key]
        public int cover_Id { get; set; }
        [Required]

        public string? cover_Name { get; set; }
    }
}
