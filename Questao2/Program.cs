using Newtonsoft.Json;
using Questao2.Application.Services;
using Questao2.Infrastructure.Repositories.Clients;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class Program
{
    private static MatchService _service;
    static Program()
    {
        var httpClient = new HttpClient();
        var repo = new FootballApiClient(httpClient);
        _service = new MatchService(repo);
    }
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        return Task.Run(() => _service.GetTotalGoalsByTeamAsync(team, year)).GetAwaiter().GetResult(); ;
    }

}