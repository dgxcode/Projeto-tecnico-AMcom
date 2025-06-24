using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Interfaces
{
    public interface IContaCorrenteQueryStore
    {
        Task<ContaCorrente> ObterContaPorNumero(int numero);
        Task<decimal> CalcularSaldo(string idContaCorrente);
    }
}
