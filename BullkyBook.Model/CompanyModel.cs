using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.Model
{
    public class CompanyModel
    {

        [Key]
        public int Comp_Id { get; set; }
        [DisplayName("Company Name")]
        public string? comp_name{ get; set; }

        [DisplayName("Company Street Address")]
        public string? comp_StAddress{ get; set; }
        [DisplayName("City")]
        public string? comp_City{ get; set; }
        [DisplayName("State")]
        public string? comp_State{ get; set; }
        [DisplayName("Postal Code")]
        public long PostalCode { get; set; }
        [DisplayName("Phone Number")]
        public long PhoneNumber { get; set; }
    }
}
