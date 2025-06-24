using Xunit;
using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Questao5.Domain.Language;
using System;

namespace Questao5.Tests.Handlers
{
    public class GetSaldoHandlerTests
    {
        private readonly IContaCorrenteQueryStore _mockQueryStore;
        private readonly GetSaldoHandler _handler;

        public GetSaldoHandlerTests()
        {
            _mockQueryStore = Substitute.For<IContaCorrenteQueryStore>();
            _handler = new GetSaldoHandler(_mockQueryStore);
        }

        [Fact]
        public async Task Deve_retornar_saldo_quando_conta_valida()
        {
            // Arrange
            var conta = new ContaCorrente
            {
                IdContaCorrente = "1",
                Numero = 123,
                Nome = "Katherine Sanchez",
                Ativo = true
            };

            _mockQueryStore.ObterContaPorNumero(123).Returns(conta);
            _mockQueryStore.CalcularSaldo("1").Returns(200);

            var request = new GetSaldoRequest { NumeroConta = 123 };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(123, result.NumeroConta);
            Assert.Equal("Katherine Sanchez", result.NomeTitular);
            Assert.Equal(200, result.Saldo);
        }

        [Fact]
        public async Task Deve_lancar_excecao_para_conta_inexistente()
        {
            _mockQueryStore.ObterContaPorNumero(999).Returns((ContaCorrente)null);
            var request = new GetSaldoRequest { NumeroConta = 999 };

            var ex = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("INVALID_ACCOUNT", ex.Tipo);
        }

        [Fact]
        public async Task Deve_lancar_excecao_para_conta_inativa()
        {
            var conta = new ContaCorrente { IdContaCorrente = "2", Numero = 852, Nome = "Jarrad", Ativo = false };
            _mockQueryStore.ObterContaPorNumero(852).Returns(conta);

            var request = new GetSaldoRequest { NumeroConta = 852 };

            var ex = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("INACTIVE_ACCOUNT", ex.Tipo);
        }
    }
}
