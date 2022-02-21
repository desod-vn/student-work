using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudentWork.Models
{
    [Table("Fund")]
    public class Fund
    {
        [Key]
        public int Id {set; get;}
    
        [Required]
        [StringLength(255)]
        public string Name {set; get;}

        [Required]
        [StringLength(255)]
        public string From {set; get;}
        
        [Required]
        [Column(TypeName="Money")]
        public decimal Price {set; get;}
    }
}
