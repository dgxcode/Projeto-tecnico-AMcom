using Questao1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao1.Domain.Interfaces
{

    public interface IContaRepository
    {
        void Salvar(ContaBancaria conta);
        ContaBancaria ObterPorNumero(int numero);
    }
}
