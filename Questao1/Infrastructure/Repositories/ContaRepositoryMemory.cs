using Questao1.Domain.Entities;
using Questao1.Domain.Interfaces;
using System.Collections.Generic;

namespace Questao1.Infrastructure.Repositories
{
    public class ContaRepositoryMemory : IContaRepository
    {
        private readonly Dictionary<int, ContaBancaria> _contas = new();

        public void Salvar(ContaBancaria conta)
        {
            _contas[conta.Numero] = conta;
        }

        public ContaBancaria ObterPorNumero(int numero)
        {
            _contas.TryGetValue(numero, out var conta);
            return conta;
        }
    }
}
