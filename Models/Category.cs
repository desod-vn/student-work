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
        public string Image {set; get;}
        
        [Required]
        [StringLength(255)]
        public string Name {set; get;}

        [Required]
        [StringLength(255)]
        public string Description {set; get;}
    }
}
