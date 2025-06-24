
using System.Collections.Generic;
using System.Threading.Tasks;
using Questao2.Domain.Entities;

namespace Questao2.Domain.Interfaces
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetMatchesByTeamAndYearAsync(string team, int year);
    }
}

