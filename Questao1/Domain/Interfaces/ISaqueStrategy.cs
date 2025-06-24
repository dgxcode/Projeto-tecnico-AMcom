using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao1.Domain.Interfaces
{
    public interface ISaqueStrategy
    {
        double CalcularTaxa(double valor);
    }
}
