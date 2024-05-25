using FluentValidation;

namespace Identity.Requests;


internal sealed record RefreshRequest(string AccessToken, string RefreshToken)
{
    
    internal class Validator : AbstractValidator<RefreshRequest>
    {
        public Validator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}