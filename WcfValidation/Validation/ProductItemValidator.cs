using FluentValidation;

namespace WcfValidation.Validation
{
    public class ProductItemValidator : AbstractValidator<ProductItem>
    {
        public ProductItemValidator()
        {
            RuleFor(item => item.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}