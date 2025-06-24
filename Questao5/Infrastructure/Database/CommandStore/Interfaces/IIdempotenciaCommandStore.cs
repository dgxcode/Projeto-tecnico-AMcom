namespace Questao5.Infrastructure.Database.CommandStore.Interfaces
{
    public interface IIdempotenciaCommandStore
    {
        Task<bool> ExisteChave(string chaveIdempotencia);
        Task SalvarResultado(string chaveIdempotencia, string requisicao, string resultado);
        Task<string> ObterResultado(string chaveIdempotencia);
    }
}


