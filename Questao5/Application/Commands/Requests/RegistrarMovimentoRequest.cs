using MediatR;
using Questao5.Application.Commands.Response;

namespace Questao5.Application.Commands.Request
{
    public class RegistrarMovimentoRequest : IRequest<RegistrarMovimentoResponse>
    {
        public string Idempotencia { get; set; } = Guid.NewGuid().ToString();
        public int NumeroConta { get; set; }
        public char TipoMovimento { get; set; } // 'C' ou 'D'
        public decimal Valor { get; set; }
    }
}
