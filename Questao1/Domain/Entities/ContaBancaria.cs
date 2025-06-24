using System.Globalization;

namespace Questao1.Domain.Entities
{
    public class ContaBancaria {
        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
        }

        public ContaBancaria(int numero, string titular, double saldo = 0.0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldo;
        }

        public void AtualizarSaldo(double novoSaldo)
        {
            Saldo = novoSaldo;
        }

        public void AtualizarTitular(string novoTitular)
        {
            Titular = novoTitular;
        }
    }
}
