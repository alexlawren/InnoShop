using InnoShop.Application.Contracts.Infrastructure;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace InnoShop.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendConfirmationEmailAsync(string toEmail, string userName, string confirmationToken)
        {
                try
                {
                    var fromAddress = _configuration["EmailSettings:From"];
                    var apiBaseUrl = _configuration["ApiSettings:BaseUrl"];

                    if (string.IsNullOrEmpty(apiBaseUrl))
                    {
                        throw new Exception("ApiSettings:BaseUrl is not configured!");
                    }
                    var confirmationLink = $"{apiBaseUrl}/api/users/confirm-email?email={Uri.EscapeDataString(toEmail)}&token={Uri.EscapeDataString(confirmationToken)}";

                    var subject = "Подтверждение регистрации в InnoShop";
                    var body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <div style='background-color: #f4f4f4; padding: 20px;'>
                            <div style='background-color: #fff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
                                <h1 style='color: #333;'>Здравствуйте, {userName}!</h1>
                                <p>Благодарим вас за регистрацию в InnoShop.</p>
                                <p>Для активации вашего аккаунта, пожалуйста, нажмите на кнопку ниже:</p>
                                <div style='text-align: center; margin: 30px 0;'>
                                    <a href='{confirmationLink}' style='background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Подтвердить Email</a>
                                </div>
                                <p style='color: #777; font-size: 12px;'>Если кнопка не работает, скопируйте ссылку: <br> {confirmationLink}</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                    await SendEmailInternalAsync(toEmail, subject, body);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send confirmation email to {Email}", toEmail);
                    throw; 
                }
            }

        public async Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken)
        {
            try
            {
                var fromAddress = _configuration["EmailSettings:From"];
                var clientBaseUrl = _configuration["ClientSettings:BaseUrl"]; 

                var resetLink = $"{clientBaseUrl}/reset-password?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(toEmail)}";

                var subject = "Сброс пароля в InnoShop";
                var body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <div style='background-color: #f4f4f4; padding: 20px;'>
                            <div style='background-color: #fff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
                                <h1 style='color: #d9534f;'>Сброс пароля</h1>
                                <p>Здравствуйте, {userName}!</p>
                                <p>Мы получили запрос на сброс пароля для вашего аккаунта.</p>
                                <div style='text-align: center; margin: 30px 0;'>
                                    <a href='{resetLink}' style='background-color: #d9534f; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Установить новый пароль</a>
                                </div>
                                <p>Ссылка действительна 1 час.</p>
                                <p style='color: #777; font-size: 12px;'>Если вы этого не делали, просто проигнорируйте письмо.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                await SendEmailInternalAsync(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", toEmail);
                throw;
            }
        }

        private async Task SendEmailInternalAsync(string toEmail, string subject, string bodyHtml)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]!);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var fromAddress = _configuration["EmailSettings:From"];

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(fromAddress));
            emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = bodyHtml };

            using var client = new SmtpClient();

            _logger.LogInformation("Connecting to SMTP {Server}:{Port} using StartTLS...", smtpServer, port);

            try
            {
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);

                _logger.LogInformation("Authenticating as {User}...", username);
                await client.AuthenticateAsync(username, password);

                _logger.LogInformation("Sending email to {Email}...", toEmail);
                await client.SendAsync(emailMessage);

                _logger.LogInformation("Email sent successfully!");

                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP Error. Server: {Server}, Port: {Port}, User: {User}", smtpServer, port, username);
                throw; 
            }
        }
    }
}