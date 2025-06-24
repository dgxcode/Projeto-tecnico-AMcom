using Questao1.Domain.Entities;
using Questao1.Domain.Exceptions;
using System.Globalization;

namespace Questao1.Application.Controllers;

public class ContaController
{
    public void Depositar(ContaBancaria conta, double valor)
    {
        var novoSaldo = conta.Saldo + valor;
        conta.AtualizarSaldo(novoSaldo);
    }

    public void Sacar(ContaBancaria conta, double valor, double taxa)
    {
        double total = valor + taxa;
        //if (conta.Saldo < total)
        //{
        //    throw new SaldoInsuficienteException(conta.Saldo, total);
        //}

        var novoSaldo = conta.Saldo - total;
        conta.AtualizarSaldo(novoSaldo);
    }

    public string ExibirConta(ContaBancaria conta)
    {
        return ($"Conta {conta.Numero}, Titular: {conta.Titular}, Saldo: $ {conta.Saldo.ToString("F2", CultureInfo.InvariantCulture)}");
    }
}
