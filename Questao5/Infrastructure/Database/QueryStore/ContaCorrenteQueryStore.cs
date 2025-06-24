using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Interfaces;
using Questao5.Infrastructure.Sqlite;
using Dapper;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class ContaCorrenteQueryStore : IContaCorrenteQueryStore
    {
        private readonly DatabaseConfig _config;

        public ContaCorrenteQueryStore(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task<ContaCorrente> ObterContaPorNumero(int numero)
        {
            using var connection = new SqliteConnection(_config.Name);

            var query = "SELECT * FROM contacorrente WHERE numero = @numero;";
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { numero });
        }

        public async Task<decimal> CalcularSaldo(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_config.Name);

            var query = @"SELECT 
                            SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END) 
                          FROM movimento 
                          WHERE idcontacorrente = @id;";

            var result = await connection.ExecuteScalarAsync<double?>(query, new { id = idContaCorrente });
            return (decimal)(result ?? 0);
        }
    }
}
