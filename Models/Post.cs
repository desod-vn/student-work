using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentWork.Models
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public int Id {set; get;}
    
        [Required]
        [StringLength(255)]
        public string Image {set; get;}
        
        [Required]
        [StringLength(255)]
        public string Name {set; get;}

        [Required]
        [Column(TypeName="Ntext")] 
        public string Content {set; get;}

        [DataType(DataType.Date)]
        public DateTime PublishDate {get; set;}

        public Category Category {set; get;}

        public User User {set; get;}
    }
}
