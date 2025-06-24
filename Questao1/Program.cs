using Questao1.Application.Controllers;
using Questao1.Application.Services;
using Questao1.Domain.Entities;
using Questao1.Infrastructure.Repositories;
using Questao1.Infrastructure.Strategies;
using System;
using System.Globalization;

namespace Questao1 {

    class Program
    {
        static void Main(string[] args)
        {
            var repo = new ContaRepositoryMemory();
            var taxaSaque = new TaxaSaqueFixa();
            var service = new ContaService(repo, taxaSaque);
            var controller = new ContaController();

            Console.Write("Entre o número da conta: ");
            int numero = int.Parse(Console.ReadLine());
            Console.Write("Entre o titular da conta: ");
            string titular = Console.ReadLine();

            double saldoInicial = 0;
            Console.Write("Haverá depósito inicial (s/n)? ");
            char resp = char.Parse(Console.ReadLine());
            if (resp == 's' || resp == 'S')
            {
                Console.Write("Entre o valor de depósito inicial: ");
                saldoInicial = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            }

            service.CriarConta(numero, titular, saldoInicial);

            Console.WriteLine("Dados da conta: ");
            Console.WriteLine(service.ExibirConta(numero));

            Console.Write("\nEntre um valor para depósito: ");
            double deposito = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            service.Depositar(numero, deposito);

            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine(service.ExibirConta(numero));

            Console.Write("\nEntre um valor para saque: ");
            double saque = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            try
            {
                service.Sacar(numero, saque);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }

            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine(service.ExibirConta(numero));

                /* Output expected:
                Exemplo 1:

                Entre o número da conta: 5447
                Entre o titular da conta: Milton Gonçalves
                Haverá depósito inicial(s / n) ? s
                Entre o valor de depósito inicial: 350.00

                Dados da conta:
                Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

                Entre um valor para depósito: 200
                Dados da conta atualizados:
                Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

                Entre um valor para saque: 199
                Dados da conta atualizados:
                Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

                Exemplo 2:
                Entre o número da conta: 5139
                Entre o titular da conta: Elza Soares
                Haverá depósito inicial(s / n) ? n

                Dados da conta:
                Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

                Entre um valor para depósito: 300.00
                Dados da conta atualizados:
                Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

                Entre um valor para saque: 298.00
                Dados da conta atualizados:
                Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
                */
        }
    }
}
