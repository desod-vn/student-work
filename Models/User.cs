using System.Collections.Generic;
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
        [MinLength(5)]
        [EmailAddress]
        public string Email {set; get;}
        
        [Required]
        [StringLength(255)]
        [MinLength(6)]
        [Display(Name = "Họ tên")]

        public string Name {set; get;}

        [Required]
        [StringLength(255)]
        [MinLength(6)]
        [Display(Name = "Mật khẩu")]

        public string Password {set; get;}

        public virtual ICollection<Post> Posts { get; set; }
    }
}
