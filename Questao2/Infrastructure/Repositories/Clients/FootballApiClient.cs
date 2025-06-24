
using Questao2.Domain.Entities;
using Questao2.Domain.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Questao2.Infrastructure.Repositories.Clients
{
    public class FootballApiClient : IMatchRepository
    {
        private readonly HttpClient _httpClient;

        public FootballApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Match>> GetMatchesByTeamAndYearAsync(string team, int year)
        {
            var allMatches = new List<Match>();
            int page = 1;
            bool morePages = true;

            while (morePages)
            {
                var url1 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={page}";
                var url2 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={page}";

                var resp1 = await GetMatchesFromUrl(url1);
                var resp2 = await GetMatchesFromUrl(url2);

                allMatches.AddRange(resp1);
                allMatches.AddRange(resp2);

                morePages = resp1.Any() || resp2.Any();
                page++;
            }

            return allMatches;
        }

        private async Task<IEnumerable<Match>> GetMatchesFromUrl(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return Enumerable.Empty<Match>();

            var json = await response.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return apiResult?.Data.Select(d => new Match
            {
                Team1 = d.Team1,
                Team2 = d.Team2,
                Team1Goals = int.Parse(d.Team1Goals),
                Team2Goals = int.Parse(d.Team2Goals),
                Year = d.Year
            }) ?? Enumerable.Empty<Match>();
        }
    }
}
