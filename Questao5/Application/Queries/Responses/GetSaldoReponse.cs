using MediatR;

namespace Questao5.Application.Queries.Response
{
    public class GetSaldoResponse
    {
        public string NomeTitular { get; set; }
        public int NumeroConta { get; set; }
        public decimal Saldo { get; set; }
        public DateTime DataHoraResposta { get; set; }
    }
}
