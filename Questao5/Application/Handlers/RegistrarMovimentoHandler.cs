using MediatR;
using Microsoft.Extensions.Logging;
using Questao5.Application.Commands.Request;
using Questao5.Application.Commands.Response;
using Questao5.Domain.Entities;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.CommandStore.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Questao5.Infrastructure.Database.CommandStore;

namespace Questao5.Application.Commands.Handlers
{
    public class RegistrarMovimentoRequestHandler : IRequestHandler<RegistrarMovimentoRequest, RegistrarMovimentoResponse>
    {
        private readonly IContaCorrenteQueryStore _contaQueryStore;
        private readonly IMovimentoCommandStore _movimentoCommandStore;
        private readonly IIdempotenciaCommandStore _idempotenciaStore;

        public RegistrarMovimentoRequestHandler(
            IContaCorrenteQueryStore contaQueryStore,
            IMovimentoCommandStore movimentoCommandStore,
            IIdempotenciaCommandStore idempotenciaStore)
        {
            _contaQueryStore = contaQueryStore;
            _movimentoCommandStore = movimentoCommandStore;
            _idempotenciaStore = idempotenciaStore;
        }

        public async Task<RegistrarMovimentoResponse> Handle(RegistrarMovimentoRequest request, CancellationToken cancellationToken)
        {
            if (await _idempotenciaStore.ExisteChave(request.Idempotencia))
            {
                var resultado = await _idempotenciaStore.ObterResultado(request.Idempotencia);
                return new RegistrarMovimentoResponse { IdMovimento = resultado };
            }

            var conta = await _contaQueryStore.ObterContaPorNumero(request.NumeroConta);
            if (conta == null)
                throw new BusinessException("Conta inexistente", "INVALID_ACCOUNT");

            if (!conta.Ativo)
                throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");

            if (request.Valor <= 0)
                throw new BusinessException("Valor inválido", "INVALID_VALUE");

            if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
                throw new BusinessException("Tipo de movimento inválido", "INVALID_TYPE");

            var movimento = new Movimento
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = conta.IdContaCorrente,
                TipoMovimento = request.TipoMovimento,
                Valor = request.Valor,
                DataMovimento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            var idMovimento = await _movimentoCommandStore.RegistrarMovimento(movimento);

            // Salva idempotência - armazenar o resultado correto
            await _idempotenciaStore.SalvarResultado(request.Idempotencia, request.ToString(), idMovimento);

            return new RegistrarMovimentoResponse { IdMovimento = idMovimento };
        }


        //public async Task<RegistrarMovimentoResponse> Handle(RegistrarMovimentoRequest request, CancellationToken cancellationToken)
        //{
        //    // Verifica se a requisição já foi processada
        //    if (await _idempotenciaStore.ExisteChave(request.Idempotencia))
        //    {
        //        var resultado = await _idempotenciaStore.ObterResultado(request.Idempotencia);
        //        return new RegistrarMovimentoResponse { IdMovimento = resultado };
        //    }

        //    // Verifica existência e status da conta
        //    var conta = await _contaQueryStore.ObterContaPorNumero(request.NumeroConta);

        //    if (conta == null)
        //        throw new BusinessException("Conta inexistente", "INVALID_ACCOUNT");

        //    if (!conta.Ativo)
        //        throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");

        //    // Valida valor
        //    if (request.Valor <= 0)
        //        throw new BusinessException("Valor inválido", "INVALID_VALUE");

        //    // Valida tipo de movimento
        //    if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
        //        throw new BusinessException("Tipo de movimento inválido", "INVALID_TYPE");

        //    // Cria novo movimento
        //    var movimento = new Movimento
        //    {
        //        IdMovimento = Guid.NewGuid().ToString(),
        //        IdContaCorrente = conta.IdContaCorrente,
        //        TipoMovimento = request.TipoMovimento,
        //        Valor = request.Valor,
        //        DataMovimento = DateTime.Now.ToString()
        //    };

        //    var idMovimento = await _movimentoCommandStore.RegistrarMovimento(movimento);

        //    // Salva idempotência
        //    await _idempotenciaStore.SalvarResultado(request.Idempotencia, "", idMovimento);

        //    return new RegistrarMovimentoResponse { IdMovimento = idMovimento };
        //}
    }
}
