using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudentWork.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id {set; get;}
    
        [Required]
        [StringLength(255)]
        public string Email {set; get;}
        
        [Required]
        [StringLength(255)]
        public string Name {set; get;}

        [Required]
        [StringLength(255)]
        public string Password {set; get;}
    }
}
