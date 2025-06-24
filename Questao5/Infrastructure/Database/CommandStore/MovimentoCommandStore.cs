using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;
using Questao5.Infrastructure.Database.CommandStore.Interfaces;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class MovimentoCommandStore : IMovimentoCommandStore
    {
        private readonly DatabaseConfig _config;

        public MovimentoCommandStore(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task<bool> IdempotenteExiste(string chave)
        {
            using var connection = new SqliteConnection(_config.Name);
            var sql = "SELECT COUNT(1) FROM idempotencia WHERE chave_idempotencia = @chave";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { chave });
            return count > 0;
        }

        public async Task SalvarIdempotencia(string chave, string requisicao, string resultado)
        {
            using var connection = new SqliteConnection(_config.Name);
            var sql = "INSERT INTO idempotencia(chave_idempotencia, requisicao, resultado) VALUES (@chave, @requisicao, @resultado)";
            await connection.ExecuteAsync(sql, new { chave, requisicao, resultado });
        }

        public async Task<string> RegistrarMovimento(Movimento movimento)
        {
            using var connection = new SqliteConnection(_config.Name);
            var id = Guid.NewGuid().ToString();

            var sql = @"INSERT INTO movimento(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                        VALUES (@id, @idconta, @data, @tipo, @valor);";

            await connection.ExecuteAsync(sql, new
            {
                id,
                idconta = movimento.IdContaCorrente,
                data = movimento.DataMovimento,
                tipo = movimento.TipoMovimento,
                valor = movimento.Valor
            });

            return id;
        }
    }
}
