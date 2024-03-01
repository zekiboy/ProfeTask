using System;
using FluentValidation;
using profe.webui.Models;

namespace profe.webui.Validators.AuthValidators
{
    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        public RegisterValidator()
        {

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(2)
                .WithName("İsim");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(2)
                .WithName("Soyadı");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(2)
                .WithName("Kullanıcı Adı");


            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(60)
                .EmailAddress()
                .MinimumLength(8)
                .WithName("E-posta Adresi");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithName("Parola");

            RuleFor(x => x.RePassword)
                .NotEmpty()
                .MinimumLength(6)
                .Equal(x => x.Password)
                .WithName("Parola Tekrarı");


        }
    }
}

