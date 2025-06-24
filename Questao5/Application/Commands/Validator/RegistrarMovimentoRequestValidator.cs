using FluentValidation;
using Questao5.Application.Commands.Request;
using Questao5.Application.Queries.Requests;

namespace Questao5.Application.Commands.Validator
{
    public class RegistrarMovimentoRequestValidator : AbstractValidator<RegistrarMovimentoRequest>
    {
        public RegistrarMovimentoRequestValidator()
        {
            RuleFor(x => x.NumeroConta)
                .GreaterThan(0)
                .WithMessage("O número da conta deve ser maior que zero.");

            RuleFor(x => x.TipoMovimento)
                .Must(tipo => tipo == 'C' || tipo == 'D')
                .WithMessage("O tipo de movimento deve ser 'C' (crédito) ou 'D' (débito).");

            RuleFor(x => x.Valor)
                .GreaterThan(0)
                .WithMessage("O valor do movimento deve ser maior que zero.");
        }
    }
}
