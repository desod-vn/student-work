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
        [Display(Name = "Họ tên")]
        public string Name {set; get;}

        [Required]
        [StringLength(255)]
        [Display(Name = "Đơn vị")]
        public string From {set; get;}
        
        [Required]
        [Column(TypeName="Money")]
        [Display(Name = "Số tiền")]
        public decimal Price {set; get;}
    }
}
