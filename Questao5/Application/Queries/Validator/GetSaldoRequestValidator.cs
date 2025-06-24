using FluentValidation;
using Questao5.Application.Queries.Requests;

namespace Questao5.Application.Queries.Validators
{
    public class GetSaldoRequestValidator : AbstractValidator<GetSaldoRequest>
    {
        public GetSaldoRequestValidator()
        {
            RuleFor(x => x.NumeroConta)
                .GreaterThan(0)
                .WithMessage("O número da conta deve ser maior que zero.");
        }
    }
}
