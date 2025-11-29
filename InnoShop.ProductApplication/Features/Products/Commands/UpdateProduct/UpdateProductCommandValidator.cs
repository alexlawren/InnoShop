using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название продукта не может быть пустым")
                .MaximumLength(100).WithMessage("Название не может превышать 100 символов");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание не может быть пустым");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Цена должна быть больше нуля")
                .Must(price => price == Math.Round(price, 2))
                .WithMessage("Цена не может иметь более 2 знаков после запятой.");
        }
    }
}
