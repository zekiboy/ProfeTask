using System;
using FluentValidation;
using profe.webui.Models;

namespace profe.webui.Validators.ProductValidators
{
	public class UpdateProductValidator : AbstractValidator<ProductModel>
    {
		public UpdateProductValidator()
		{


            RuleFor(x => x.Name)
             .NotEmpty()
             .WithName("İsim");


            RuleFor(x => x.Description)
                .NotEmpty()
                .WithName("Açıklama");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithName("Fiyat");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithName("Stok");

            RuleFor(x => x.IsApproved)
                .NotEmpty()
                .WithName("IsApproved");


            //RuleFor(x => x.SelectedCategories)
            //    .NotEmpty()
            //    .Must(categories => categories.Any())
            //    .WithName("Kategoriler");

        }
    }
}

