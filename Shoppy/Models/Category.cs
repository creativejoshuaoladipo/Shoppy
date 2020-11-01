using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="The Category Name is Required")]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="The Display Order must be greater than 1")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
