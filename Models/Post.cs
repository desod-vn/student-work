using Microsoft.AspNetCore.Http;
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
    
        //[Required]
        [StringLength(255)]
        [Display(Name = "Hình ảnh")]
        public string Image {set; get;}

        [NotMapped]
        [Required]
        [Display(Name = "Hình ảnh")]
        public IFormFile ImageFile { get; set; }
        
        [Required]
        [StringLength(255)]
        [Display(Name = "Tên")]
        [MinLength(10)]
        public string Name {set; get;}

        [Required]
        [Column(TypeName="Ntext")]
        [Display(Name = "Nội dung")]
        [MinLength(300)]
        public string Content {set; get;}

        [DataType(DataType.Date)]
        [Display(Name = "Ngày đăng")]
        public DateTime PublishDate {get; set;}

        [Display(Name = "Thể loại")]
        public int CategoryId { get; set; }

        [Display(Name = "Thể loại")]
        public Category Category {set; get;}

        public int UserId { get; set; }
        public User User {set; get;}
    }
}
