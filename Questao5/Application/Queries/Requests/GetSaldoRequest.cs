using MediatR;
using Questao5.Application.Queries.Response;

namespace Questao5.Application.Queries.Requests
{
    public class GetSaldoRequest : IRequest<GetSaldoResponse>
    {
        public int NumeroConta { get; set; }
    }
}
