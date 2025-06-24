//using Xunit;
//using Questao1.Domain.Entities;
//using Questao1.Application.Controllers;

//namespace Questao1.Tests
//{
//    public class ContaControllerTests
//    {
//        private readonly ContaController _controller;

//        public ContaControllerTests()
//        {
//            _controller = new ContaController();
//        }

//        [Fact]
//        public void Deve_Criar_Conta_Com_Deposito_Inicial()
//        {
//            // Arrange
//            var conta = new ContaBancaria(5447, "Milton Gonçalves", 350.00);

//            // Assert
//            Assert.Equal(5447, conta.Numero);
//            Assert.Equal("Milton Gonçalves", conta.Titular);
//            Assert.Equal(350.00, conta.Saldo, 2);
//        }

//        [Fact]
//        public void Deve_Criar_Conta_Sem_Deposito_Inicial()
//        {
//            // Arrange
//            var conta = new ContaBancaria(5139, "Elza Soares");

//            // Assert
//            Assert.Equal(5139, conta.Numero);
//            Assert.Equal("Elza Soares", conta.Titular);
//            Assert.Equal(0.00, conta.Saldo, 2);
//        }

//        [Fact]
//        public void Deve_Realizar_Deposito()
//        {
//            var conta = new ContaBancaria(5447, "Milton Gonçalves", 350.00);

//            _controller.Depositar(conta, 200.00);

//            Assert.Equal(550.00, conta.Saldo, 2);
//        }

//        [Fact]
//        public void Deve_Realizar_Saque_Com_Taxa()
//        {
//            var conta = new ContaBancaria(5447, "Milton Gonçalves", 550.00);

//            _controller.Sacar(conta, 199.00, 3.50);

//            Assert.Equal(347.50, conta.Saldo, 2);
//        }

//        [Fact]
//        public void Deve_Permitir_Saque_Que_Deixa_Saldo_Negativo()
//        {
//            var conta = new ContaBancaria(5139, "Elza Soares");

//            _controller.Depositar(conta, 300.00);
//            _controller.Sacar(conta, 298.00, 3.50);

//            Assert.Equal(-1.50, conta.Saldo, 2);
//        }

//        [Fact]
//        public void Deve_Exibir_Dados_Corretamente()
//        {
//            var conta = new ContaBancaria(5447, "Milton Gonçalves", 350.00);

//            var resultado = _controller.ExibirConta(conta);

//            Assert.Equal("Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00", resultado);
//        }

//        [Fact]
//        public void Deve_Atualizar_Titular()
//        {
//            var conta = new ContaBancaria(9999, "Maria Silva");

//            conta.AtualizarTitular("Maria Oliveira");

//            Assert.Equal("Maria Oliveira", conta.Titular);
//        }
//    }
//}
