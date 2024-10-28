using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Contracts.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string to,string subject,string body);
    }
}
