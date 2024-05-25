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
            var builder = new BodyBuilder { HtmlBody = htmlMessage };
            var emailMessage = new MimeMessage { Subject = subject, Body = builder.ToMessageBody() };
            emailMessage.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.DefaultFromEmail));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port, useSsl: true);
            await client.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "There was an error in sending mail");
        }
    }

    public Task SendRegisterConfirmationAsync(string email, string code, string returnUrl) =>
        SendTemplate(email, code, returnUrl, "Register.html", "Registration");

    public Task SendPasswordResetCodeAsync(string email, string code, string returnUrl) =>
        SendTemplate(email, code, returnUrl, "PasswordReset.html", "Password Reset");

    public Task SendEmailChangeConfirmationAsync(string email, string code, string returnUrl) =>
        SendTemplate(email, code, returnUrl, "EmailChange.html", "Email Change");

    private async Task SendTemplate(string email, string code, string returnUrl, string filename, string subject)
    {
        var path = Path.Combine(_emailOptions.TemplatesFolder, filename);
        var template = await File.ReadAllTextAsync(path);
        var link = $"{returnUrl}?email={HtmlEncoder.Default.Encode(email)}&code={HtmlEncoder.Default.Encode(code)}";
        var htmlMessage = string.Format(template, link);
        await SendEmailAsync(email, subject, htmlMessage);
    }
}