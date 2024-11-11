using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Models
{
    public class Category
    {
        /**
         * [TT]: EF data annotation attribute for primary key.
         */
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category name")]
        [MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Display order")]
        [Range(1, 100)]
        //[Range(1, 100, ErrorMessage = "")]
        public int DisplayOrder { get; set; }
    }
}
