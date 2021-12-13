using FluentValidation;
using Models;

namespace AspFirstMVC.Validator
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(r => r.Id).NotNull().NotEmpty().GreaterThan(0).WithMessage("No puede vacío ni menor o igual a 0"); ;
            RuleFor(r => r.UserId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(r => r.Title).NotNull().NotEmpty().MaximumLength(20).MinimumLength(1);
        }
    }
}
