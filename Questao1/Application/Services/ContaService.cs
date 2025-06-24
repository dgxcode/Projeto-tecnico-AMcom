using Questao1.Application.Controllers;
using Questao1.Domain.Entities;
using Questao1.Domain.Exceptions;
using Questao1.Domain.Interfaces;

namespace Questao1.Application.Services
{

    public class ContaService
    {
        private readonly IContaRepository _repo;
        private readonly ISaqueStrategy _taxaSaque;
        private readonly ContaController _controller;

        public ContaService(IContaRepository repo, ISaqueStrategy taxaSaque)
        {
            _repo = repo;
            _taxaSaque = taxaSaque;
            _controller = new ContaController();
        }

        public void CriarConta(int numero, string titular, double? depositoInicial)
        {
            var existeConta = _repo.ObterPorNumero(numero);
            if (existeConta != null)
            {
                throw new ContaExisteException(numero);
            }

            var conta = new ContaBancaria(numero, titular, depositoInicial ?? 0.0);
            _repo.Salvar(conta);
        }

        public void Depositar(int numeroConta, double valor)
        {
            var conta = _repo.ObterPorNumero(numeroConta);
            _controller.Depositar(conta, valor);
        }

        public void Sacar(int numeroConta, double valor)
        {
            var conta = _repo.ObterPorNumero(numeroConta);
            var taxa = _taxaSaque.CalcularTaxa(valor);
            _controller.Sacar(conta, valor, taxa);
        }

        public string ExibirConta(int numeroConta)
        {
            var conta = _repo.ObterPorNumero(numeroConta);
            return _controller.ExibirConta(conta);
        }

        public ContaBancaria ObterPorNumero(int numeroConta)
        {
            return _repo.ObterPorNumero(numeroConta);
        }
    }

}
