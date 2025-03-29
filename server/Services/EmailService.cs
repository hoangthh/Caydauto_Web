using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailService : IEmailSender
{
    // Biến thành viên để lưu trữ cấu hình
    private readonly IConfiguration _configuration;

    // Hàm khởi tạo, nhận vào một đối tượng IConfiguration
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Phương thức gửi email không đồng bộ
    public async Task<bool> SendEmailAsync(string email, string subject, string callBackUrl)
    {
        try
        {
            // Lấy cấu hình email từ file cấu hình
            var emailSettings = _configuration.GetSection("EmailSettings");
            var body = GenerateBodyEmail(callBackUrl); // Tạo nội dung email với mã OTP
            // Lấy thông tin cấu hình SMTP
            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587"); // Nếu null thì lấy 587
            var smtpUser = emailSettings["SmtpUser"];
            var smtpPass = emailSettings["SmtpPass"];
            var senderName = emailSettings["SenderName"];
            var senderEmail = emailSettings["SenderEmail"] ?? "No Indentify"; // Nếu null thì lấy "No Indentify"

            // Tạo đối tượng SmtpClient với thông tin cấu hình
            var client = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
            };

            // Tạo đối tượng MailMessage với thông tin email
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            // Thêm địa chỉ email người nhận
            mailMessage.To.Add(new MailAddress(email));

            // Gửi email không đồng bộ
            await client.SendMailAsync(mailMessage);
            // Trả về true nếu gửi thành công
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private string GenerateBodyEmail(string callBackUrl)
    // Phương thức tạo nội dung email với mã OTP
    {
        var body =
            $@"<div style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 30px;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1); overflow: hidden;'>
                <!-- Header -->
                <div style='background: linear-gradient(to right, #4CAF50, #81C784); padding: 20px; text-align: center;'>
                    <h2 style='color: #ffffff; margin: 0; font-size: 24px;'>Confirm Your Email</h2>
                </div>
                <!-- Body -->
                <div style='padding: 30px;'>
                    <p style='font-size: 16px; color: #444; text-align: center; margin: 0 0 20px;'>Dear User,</p>
                    <p style='font-size: 16px; color: #666; text-align: center; line-height: 1.5;'>
                        Please click the link below to confirm your email address and activate your account:
                    </p>
                    <div style='text-align: center; margin: 25px 0;'>
                        <a href='{callBackUrl}' style='display: inline-block; padding: 12px 24px; background-color: #4CAF50; color: #ffffff; text-decoration: none; font-size: 16px; font-weight: bold; border-radius: 25px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);'>
                            Click Here
                        </a>
                    </div>
                    <p style='font-size: 14px; color: #888; text-align: center;'>
                        This link will expire in 5 minutes. If you didn’t request this, feel free to ignore this email.
                    </p>
                </div>
                <!-- Footer -->
                <div style='background: linear-gradient(to right, #4CAF50, #81C784); padding: 15px; text-align: center; color: #ffffff;'>
                    <p style='margin: 0; font-size: 14px;'>Thank you for choosing us!</p>
                    <p style='margin: 5px 0 0; font-size: 16px; font-weight: bold;'>CayDauTo</p>
                </div>
            </div>
        </div>";
        return body;
    }

    Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return SendEmailAsync(email, subject, htmlMessage);
    }
}
