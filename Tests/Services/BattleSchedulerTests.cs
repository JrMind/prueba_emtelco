using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class BattleSchedulerTests
{
    [Fact]
    public async Task ScheduleBattles_ReturnsCorrectNumber()
    {
        var characters = Enumerable.Range(1, 4).Select(i => new Character { Id = i, Name = $"Fighter{i}" }).ToList();
        var serviceMock = new Mock<IExternalCharacterService>();
        serviceMock.Setup(s => s.GetCharactersAsync(4)).ReturnsAsync(characters);
        var scheduler = new BattleScheduler(serviceMock.Object);

        var result = await scheduler.ScheduleBattlesAsync(4);

        Assert.Equal(2, result.Count);
        Assert.All(result, r => Assert.Contains("Fighter", r.Fighter1));
    }
}
