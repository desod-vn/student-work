using Microsoft.EntityFrameworkCore;

namespace StudentWork
{
    public class StudentWorkContext : DbContext
    {
        public StudentWorkContext(DbContextOptions<StudentWorkContext> options) : base(options)
        {
            // Phương thức khởi tạo này chứa options để kết nối đến MS SQL Server
            // Thực hiện điều này khi Inject trong dịch vụ hệ thống
        }
    }
}