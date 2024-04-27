using System.Text.Encodings.Web;
using Identity.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Identity.Services;

internal sealed class EmailSender(ILogger<EmailSender> logger, IOptions<EmailOptions> emailOptions) : IEmailSender
{
    private readonly EmailOptions _emailOptions = emailOptions.Value;

    private async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        logger.LogInformation("Sending email to {Email} with subject {Subject}", email, subject);
        try
        {
            BodyBuilder builder = new()
            {
                HtmlBody = htmlMessage
            };
            var emailMessage = new MimeMessage
            {
                Subject = subject,
                Body = builder.ToMessageBody()
            };
            emailMessage.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.DefaultFromEmail));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port, useSsl: true).ConfigureAwait(false);
            await client.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password).ConfigureAwait(false);
            await client.SendAsync(emailMessage).ConfigureAwait(false);
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "There was an error in sending mail");
        }
    }

    public async Task SendRegisterConfirmationAsync(string email, string code, string returnUrl)
    {
        var path = Path.Combine(_emailOptions.TemplatesFolder, "Register.html");
        var template = await File.ReadAllTextAsync(path).ConfigureAwait(false);
        var link = $"{returnUrl}?email={HtmlEncoder.Default.Encode(email)}&code={HtmlEncoder.Default.Encode(code)}";
        var htmlMessage = string.Format(template, link);
        await SendEmailAsync(email, "Registration", htmlMessage).ConfigureAwait(false);
    }

    public async Task SendPasswordResetCodeAsync(string email, string code, string returnUrl)
    {
        var path = Path.Combine(_emailOptions.TemplatesFolder, "PasswordReset.html");
        var template = await File.ReadAllTextAsync(path).ConfigureAwait(false);
        var link = $"{returnUrl}?email={HtmlEncoder.Default.Encode(email)}&code={HtmlEncoder.Default.Encode(code)}";
        var htmlMessage = string.Format(template, link);
        await SendEmailAsync(email, "Password Reset", htmlMessage).ConfigureAwait(false);
    }

    public async Task SendEmailChangeConfirmationAsync(string email, string code, string returnUrl)
    {
        var path = Path.Combine(_emailOptions.TemplatesFolder, "EmailChange.html");
        var template = await File.ReadAllTextAsync(path).ConfigureAwait(false);
        var link = $"{returnUrl}?email={HtmlEncoder.Default.Encode(email)}&code={HtmlEncoder.Default.Encode(code)}";
        var htmlMessage = string.Format(template, link);
        await SendEmailAsync(email, "Email Change", htmlMessage).ConfigureAwait(false);
    }
}