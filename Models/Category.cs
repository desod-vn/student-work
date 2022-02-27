using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentWork.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id {set; get;}
    
        [StringLength(255)]
        [Display(Name = "Hình ảnh")]
        public string Image {set; get;}
        
        [Required]
        [StringLength(255)]
        [Display(Name = "Tên chuyên mục")]
        public string Name {set; get;}

        [Required]
        [StringLength(255)]
        [Display(Name = "Mô tả")]
        public string Description {set; get;}
    }
}
