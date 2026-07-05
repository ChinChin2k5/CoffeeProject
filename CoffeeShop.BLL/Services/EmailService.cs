using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using CoffeeShop.BLL.DTOs.Inventory;

public class EmailService 
{
    //Ờm, trước đây mình nên khai báo những chỗ mình nên sử dụng 
    public async Task SendOtpEmailAsync(string toEmail, string otpCode)
    {
        //1. Tạo đối tượng Email 
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("HetCuuAdmin","hetcuu@gmail.com"));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = "Đây là mã OTP của bạn !";
        //2. Viết ruột thư
        message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = $"<h3>Mã OTP của bạn là: <b style='color:red;'>{otpCode}</b></h3>" +
                   $"<p>Mã này chỉ có hiệu lực 15 phút</p>"
        };
        //3. Mang đi gửi 
        using (var client = new SmtpClient())
        {
            // Kết nối tới cục server SMTP của Gmail
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            // Xác thực tài khoản
            await client.AuthenticateAsync("hetcuu@gmail.com", "123456");
            // Khởi động
            await client.SendAsync(message);
            // Kết thúc 
            await client.DisconnectAsync(true);
        }
    }
}