using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.CommandStore.Interfaces;
using Questao5.Infrastructure.Sqlite;
using Dapper;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class IdempotenciaCommandStore : IIdempotenciaCommandStore
    {
        private readonly DatabaseConfig _config;

        public IdempotenciaCommandStore(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task<bool> ExisteChave(string chaveIdempotencia)
        {
            using var conn = new SqliteConnection(_config.Name);
            var result = await conn.QuerySingleOrDefaultAsync<string>(
                "SELECT chave_idempotencia FROM idempotencia WHERE chave_idempotencia = @chave",
                new { chave = chaveIdempotencia });

            return result != null;
        }

        //public async Task SalvarResultado(string chaveIdempotencia, string requisicao, string resultado)
        //{
        //    using var conn = new SqliteConnection(_config.Name);
        //    await conn.ExecuteAsync(
        //        "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@chave, @req, @res)",
        //        new { chave = chaveIdempotencia, req = requisicao, res = resultado });
        //}

        public async Task SalvarResultado(string chaveIdempotencia, string requisicao, string resultado)
        {
            using var conn = new SqliteConnection(_config.Name);

            await conn.ExecuteAsync(@"
                                    INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                                    VALUES (@chave, @req, @res)
                                    ON CONFLICT(chave_idempotencia) DO UPDATE SET
                                        requisicao = excluded.requisicao,
                                        resultado = excluded.resultado;",
                new { chave = chaveIdempotencia, req = requisicao, res = resultado });
        }

        public async Task<string> ObterResultado(string chaveIdempotencia)
        {
            using var conn = new SqliteConnection(_config.Name);
            return await conn.QuerySingleOrDefaultAsync<string>(
                "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @chave",
                new { chave = chaveIdempotencia });
        }
    }

}
