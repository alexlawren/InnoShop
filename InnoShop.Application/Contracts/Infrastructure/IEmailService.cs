using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string userName, string confirmationToken);
        Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetToken);
    }
}
