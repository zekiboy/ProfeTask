using System;
namespace profe.webui.Data.EmailService
{
	public interface IEmailSender
	{
        Task SendEmailAsync(string email, string subject, string htmlMessage);

    }
}

