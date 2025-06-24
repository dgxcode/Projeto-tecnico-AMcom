using System;

namespace Questao1.Domain.Exceptions
{
    public class ContaBancariaException : Exception
    {
        public ContaBancariaException(string message) : base(message) { }
    }

    public class SaldoInsuficienteException : ContaBancariaException
    {
        public SaldoInsuficienteException(double saldo, double totalSolicitado)
            : base($"Saldo insuficiente. Saldo atual: R$ {saldo:F2}, valor solicitado com taxa: R$ {totalSolicitado:F2}")
        {
        }
    }

    public class ContaExisteException : ContaBancariaException
    {
        public ContaExisteException(int numero)
            : base($"Já existe uma conta com o número {numero}.")
        {
        }
    }
}
