using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyCafeProject.API.Models 
{
    /// <summary>
    /// Bảng chứa thông tin tài khoản đăng nhập hệ thống.
    /// Không chứa thông tin chi tiết (Profile) như ngày sinh, địa chỉ (nên tách bảng khác).
    /// </summary>
    [Table("Users")] // Mapping cứng tên bảng trong SQL là "Users" (số nhiều) cho chuẩn chuẩn
    public class User 
    {
        [Key]
        public int Id {get; set;} // Khóa chính, tự động tăng (Identity)
        /// <summary>
        /// Tên đăng nhập của quản lý.
        /// </summary>
        [Required(ErrorMessage = "Tên Đăng Nhập Không Được Để Trống !")] // Thêm ErrorMessage để sau này FE hiển thị lỗi
        [MaxLength(50)] // Giới hạn 50 ký tự để tiết kiệm bộ nhớ Database (thay vì text vô tận)
        public string UserName {get; set;} = string.Empty; // Gán = "" để tránh warning Nullable của .NET 9
        /// <summary>
        /// Mật khẩu ĐÃ ĐƯỢC MÃ HÓA (Bcrypt/Argon2).
        /// TUYỆT ĐỐI KHÔNG LƯU PASSWORD DẠNG TEXT (PLAIN TEXT).
        /// </summary>
        [Required]
        [MaxLength(255)] // Hash thường dài, 255 là an toàn
        public string PasswordHash {get; set;} = string.Empty;
    }
}