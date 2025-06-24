using Questao1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao1.Infrastructure.Strategies
{
    public class TaxaSaqueFixa : ISaqueStrategy
    {
        public double CalcularTaxa(double valor) => 3.50;
    }
}
