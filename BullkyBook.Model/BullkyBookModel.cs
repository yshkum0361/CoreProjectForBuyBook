using System.ComponentModel;

using System.ComponentModel.DataAnnotations;

namespace BullkyBook.Model
{
    public class BullkyBookModel
    {
        [Key]
        public int book_Id { get; set; }
        [Required]

        public string? book_Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 1000, ErrorMessage = "The value is out of range!!!")]
        public int display_Order { get; set; }
        public DateTime oder_date { get; set; } = DateTime.Now;
    }
}
