using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Identity.Extensions;

internal static class RuleBuilderOptionsExtensions
{
    public static IRuleBuilderOptions<T, string> IsValidUsername<T>(this IRuleBuilderInitial<T, string> ruleBuilder) =>
        ruleBuilder.Cascade(CascadeMode.Stop).NotEmpty().Length(3, 20);

    public static IRuleBuilderOptions<T, string> IsValidEmail<T>(this IRuleBuilderInitial<T, string> ruleBuilder) =>
        ruleBuilder.Cascade(CascadeMode.Stop).NotEmpty().EmailAddress().MaximumLength(byte.MaxValue);

    public static IRuleBuilderOptions<T, string> IsValidPassword<T>(this IRuleBuilderInitial<T, string> ruleBuilder,
        PasswordOptions passwordOptions)
    {
        var builderOptions = ruleBuilder.NotEmpty();

        if (passwordOptions.RequireDigit)
            builderOptions
                .Must(x => x.Any(c => c is >= '0' and <= '9'))
                .WithMessage(PasswordValidationErrors.RequireDigit);

        if (passwordOptions.RequireLowercase)
            builderOptions
                .Must(x => x.Any(c => c is >= 'a' and <= 'z'))
                .WithMessage(PasswordValidationErrors.RequireLowercase);

        if (passwordOptions.RequireUppercase)
            builderOptions
                .Must(x => x.Any(c => c is >= 'A' and <= 'Z'))
                .WithMessage(PasswordValidationErrors.RequireUppercase);

        if (passwordOptions.RequireNonAlphanumeric)
            builderOptions
                .Must(x => !x.All(c => c is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9'))
                .WithMessage(PasswordValidationErrors.RequireNonAlphanumeric);


        return builderOptions.MinimumLength(passwordOptions.RequiredLength);
    }
}

public static class PasswordValidationErrors
{
    public static string RequireDigit => "Password must contain a digit";
    public static string RequireLowercase => "Password must contain a lower case character";
    public static string RequireUppercase => "Password must contain a upper case character";
    public static string RequireNonAlphanumeric => "Password must contain a non-alphanumeric character";
}