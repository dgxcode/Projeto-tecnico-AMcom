using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Response;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.QueryStore.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Handlers
{
    public class GetSaldoHandler : IRequestHandler<GetSaldoRequest, GetSaldoResponse>
    {
        private readonly IContaCorrenteQueryStore _queryStore;

        public GetSaldoHandler(IContaCorrenteQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<GetSaldoResponse> Handle(GetSaldoRequest request, CancellationToken cancellationToken)
        {
            var conta = await _queryStore.ObterContaPorNumero(request.NumeroConta);

            if (conta == null)
                throw new BusinessException("Conta inexistente", "INVALID_ACCOUNT");

            if (!conta.Ativo)
                throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");

            var saldo = await _queryStore.CalcularSaldo(conta.IdContaCorrente);

            return new GetSaldoResponse
            {
                NomeTitular = conta.Nome,
                NumeroConta = conta.Numero,
                Saldo = saldo,
                DataHoraResposta = DateTime.Now
            };
        }
    }
}
