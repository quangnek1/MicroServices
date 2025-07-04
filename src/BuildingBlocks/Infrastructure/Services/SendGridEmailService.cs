using Contracts.Services;
using Shared.Services.Email;

namespace Infrastructure.Services
{
	public class SendGridEmailService : IEmailService<SendGridEmailRequest>
	{
		public Task SendEmailAsync(SendGridEmailRequest request, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
