using Microsoft.EntityFrameworkCore;
using StudentWork.Models;

namespace StudentWork
{
    public class StudentWorkContext : DbContext
    {
        public StudentWorkContext(DbContextOptions<StudentWorkContext> options) : base(options)
        {
            // Phương thức khởi tạo này chứa options để kết nối đến MS SQL Server
            // Thực hiện điều này khi Inject trong dịch vụ hệ thống
        }

        public DbSet<User> users {set; get;}
        public DbSet<Post> posts {set; get;}
        public DbSet<Category> categories {set; get;}
        public DbSet<Fund> funds {set; get;}
        public DbSet<Setting> settings {set; get;}
    }
}