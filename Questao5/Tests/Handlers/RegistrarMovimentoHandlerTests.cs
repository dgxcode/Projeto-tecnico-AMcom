using NSubstitute;
using Questao5.Application.Commands.Handlers;
using Questao5.Application.Commands.Request;
using Questao5.Application.Commands.Response;
using Questao5.Domain.Entities;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.CommandStore.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Questao5.Tests.Handlers
{
    public class RegistrarMovimentoHandlerTests
    {
        private readonly IContaCorrenteQueryStore _queryStore;
        private readonly IMovimentoCommandStore _commandStore;
        private readonly IIdempotenciaCommandStore _idempotenciaStore;
        private readonly RegistrarMovimentoRequestHandler _handler;
        private readonly DatabaseConfig _config;
        private readonly IDatabaseBootstrap _bootstrap;

        public RegistrarMovimentoHandlerTests()
        {
            _config = new DatabaseConfig { Name = "Data Source=database.sqlite" };
            _bootstrap = new DatabaseBootstrap(_config);
            _bootstrap.Setup();
            _queryStore = Substitute.For<IContaCorrenteQueryStore>();
            _commandStore = Substitute.For<IMovimentoCommandStore>();
            _idempotenciaStore = Substitute.For<IIdempotenciaCommandStore>();
            _handler = new RegistrarMovimentoRequestHandler(_queryStore, _commandStore, _idempotenciaStore);
        }

        [Fact]
        public async Task Deve_registrar_movimento_valido()
        {
            var conta = new ContaCorrente { IdContaCorrente = "abc123", Numero = 123, Nome = "Katherine", Ativo = true };
            var request = new RegistrarMovimentoRequest
            {
                Idempotencia = "req-123",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100
            };

            _idempotenciaStore.ExisteChave("req-123").Returns(false);
            _queryStore.ObterContaPorNumero(123).Returns(conta);
            _idempotenciaStore.ObterResultado("req-123").Returns((string)null);

            _commandStore
                .RegistrarMovimento(Arg.Is<Movimento>(m =>
                    m.IdContaCorrente == conta.IdContaCorrente &&
                    m.TipoMovimento == 'C' &&
                    m.Valor == 100
                ))
                .Returns("mov-001");

            var response = await _handler.Handle(request, CancellationToken.None);

            Assert.Equal("mov-001", response.IdMovimento);
        }


        [Fact]
        public async Task Deve_retornar_resultado_de_idempotencia_existente()
        {
            var request = new RegistrarMovimentoRequest
            {
                Idempotencia = "req-123",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 100
            };

            _idempotenciaStore.ExisteChave("req-123").Returns(true);
            _idempotenciaStore.ObterResultado("req-123").Returns("mov-xyz");

            var response = await _handler.Handle(request, CancellationToken.None);

            Assert.Equal("mov-xyz", response.IdMovimento);
        }


        [Fact]
        public async Task Deve_lancar_excecao_para_conta_inexistente()
        {
            var request = new RegistrarMovimentoRequest
            {
                Idempotencia = "abc",
                NumeroConta = 999,
                TipoMovimento = 'D',
                Valor = 50
            };

            _queryStore.ObterContaPorNumero(999).Returns((ContaCorrente)null);

            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal("INVALID_ACCOUNT", ex.Tipo);
        }

        [Fact]
        public async Task Deve_lancar_excecao_para_conta_inativa()
        {
            var conta = new ContaCorrente { IdContaCorrente = "xyz", Numero = 852, Ativo = false };
            var request = new RegistrarMovimentoRequest
            {
                Idempotencia = "abc",
                NumeroConta = 852,
                TipoMovimento = 'D',
                Valor = 50
            };

            _queryStore.ObterContaPorNumero(852).Returns(conta);

            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal("INACTIVE_ACCOUNT", ex.Tipo);
        }

        [Fact]
        public async Task Deve_lancar_excecao_para_valor_zero_ou_negativo()
        {
            var conta = new ContaCorrente { IdContaCorrente = "abc123", Numero = 123, Ativo = true };

            var request = new RegistrarMovimentoRequest
            {
                Idempotencia = "abc",
                NumeroConta = 123,
                TipoMovimento = 'C',
                Valor = 0
            };

            _queryStore.ObterContaPorNumero(123).Returns(conta);

            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal("INVALID_VALUE", ex.Tipo);
        }

        [Fact]
        public async Task Deve_lancar_excecao_para_tipo_invalido()
        {
            var conta = new ContaCorrente { IdContaCorrente = "abc123", Numero = 123, Ativo = true };

            var request = new RegistrarMovimentoRequest
            {
                Idempotencia = "abc",
                NumeroConta = 123,
                TipoMovimento = 'X', // inválido
                Valor = 100
            };

            _queryStore.ObterContaPorNumero(123).Returns(conta);

            var ex = await Assert.ThrowsAsync<BusinessException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal("INVALID_TYPE", ex.Tipo);
        }
    }
}
