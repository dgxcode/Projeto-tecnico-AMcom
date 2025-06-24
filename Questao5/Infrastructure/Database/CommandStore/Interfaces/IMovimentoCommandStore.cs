using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Interfaces
{
    public interface IMovimentoCommandStore
    {
        Task<string> RegistrarMovimento(Movimento movimento);
        Task<bool> IdempotenteExiste(string chave);
        Task SalvarIdempotencia(string chave, string requisicao, string resultado);
    }
}
