using Moq;
using Xunit;
// Alias para a entidade Match do seu domínio
using DomainMatch = Questao2.Domain.Entities.Match;
using Questao2.Application.Services;
using Questao2.Domain.Interfaces;

namespace Questao2.Tests
{
    public class MatchServiceTests
    {
        [Fact]
        public async Task GetTotalGoalsByTeamAsync_ShouldReturnCorrectTotal_WhenTeamMatchesFound()
        {
            var mockRepo = new Mock<IMatchRepository>();
            mockRepo.Setup(repo => repo.GetMatchesByTeamAndYearAsync("Chelsea", 2014))
                .ReturnsAsync(new List<DomainMatch>
                {
                    new DomainMatch { Team1 = "Chelsea", Team1Goals = 2, Team2 = "TeamB", Team2Goals = 1, Year = 2014 },
                    new DomainMatch { Team1 = "TeamC", Team1Goals = 1, Team2 = "Chelsea", Team2Goals = 3, Year = 2014 },
                    new DomainMatch { Team1 = "Chelsea", Team1Goals = 4, Team2 = "TeamD", Team2Goals = 0, Year = 2014 }
                });

            var service = new MatchService(mockRepo.Object);

            var totalGoals = await service.GetTotalGoalsByTeamAsync("Chelsea", 2014);
            Assert.Equal(9, totalGoals); // 2 + 3 + 4
        }

        [Fact]
        public async Task GetTotalGoalsByTeamAsync_ShouldReturnZero_WhenNoMatchesFound()
        {
            var mockRepo = new Mock<IMatchRepository>();
            mockRepo.Setup(repo => repo.GetMatchesByTeamAndYearAsync("Unknown", 2020))
                .ReturnsAsync(new List<DomainMatch>());

            var service = new MatchService(mockRepo.Object);

            var totalGoals = await service.GetTotalGoalsByTeamAsync("Unknown", 2020);

            Assert.Equal(0, totalGoals);
        }
    }
}
