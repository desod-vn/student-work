using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudentWork.Models
{
    [Table("Setting")]
    public class Setting
    {
        [Key]
        public int Id {set; get;}
    
        [Required]
        [StringLength(255)]
        public string Key {set; get;}
        
        [Required]
        [StringLength(255)]
        public string Value {set; get;}
    }
}
