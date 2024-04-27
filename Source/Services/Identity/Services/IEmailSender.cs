namespace Identity.Services;

internal interface IEmailSender
{
    Task SendRegisterConfirmationAsync(string email, string code, string returnUrl);
    Task SendPasswordResetCodeAsync(string email, string code, string resetUrl);
    Task SendEmailChangeConfirmationAsync(string email, string code, string returnUrl);
}