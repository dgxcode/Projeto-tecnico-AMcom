using Questao2.Domain.Entities;
using Questao2.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questao2.Application.Services
{
    public class MatchService
    {
        private readonly IMatchRepository _repo;

        public MatchService(IMatchRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> GetTotalGoalsByTeamAsync(string team, int year)
        {
            var matches = await _repo.GetMatchesByTeamAndYearAsync(team, year);
            int totalGoals = 0;

            foreach (var match in matches)
            {
                if (match.Team1 == team)
                    totalGoals += match.Team1Goals;
                else if (match.Team2 == team)
                    totalGoals += match.Team2Goals;
            }

            return totalGoals;
        }
    }
}
