using FluentValidation;
using ProductService.Dto.ProductDto;

namespace ProductService.Services.DtoValidators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(50).WithMessage("Product name must not exceed 100 characters");

        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(300);

        RuleFor(x => x.ProductPrice)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be 3 characters (e.g., USD)");
    }
}