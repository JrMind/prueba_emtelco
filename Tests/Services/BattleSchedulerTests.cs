using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Tests.Services;

public class BattleSchedulerTests
{
    [Fact]
    public async Task ScheduleBattles_ReturnsCorrectNumberOfBattles()
    {
        // Arrange
        var characters = Enumerable.Range(1, 4)
            .Select(i => new Character { Id = i, Name = $"Fighter{i}" })
            .ToList();

        var serviceMock = new Mock<IExternalCharacterService>();
        serviceMock.Setup(s => s.GetCharacterAsync(4)).ReturnsAsync(characters);

        var scheduler = new BattleScheduler(serviceMock.Object);

        // Act
        var result = await scheduler.ScheduleBattlesAsync(4);

        // Assert
        Assert.Equal(2, result.Count); // 4 personajes = 2 batallas
        Assert.All(result, b => Assert.Matches(@"Fighter\d+ vs Fighter\d+", b.Batalla));
        Assert.All(result, b => Assert.Matches(@"\d{4}/\d{2}/\d{2}", b.Fecha));
    }

    [Fact]
    public async Task ScheduleBattles_ThrowsArgumentException_ForInvalidInput()
    {
        var serviceMock = new Mock<IExternalCharacterService>();
        var scheduler = new BattleScheduler(serviceMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => scheduler.ScheduleBattlesAsync(3)); 
        await Assert.ThrowsAsync<ArgumentException>(() => scheduler.ScheduleBattlesAsync(0)); 
        await Assert.ThrowsAsync<ArgumentException>(() => scheduler.ScheduleBattlesAsync(18)); 
    }

    [Fact]
    public async Task ScheduleBattles_ThrowsException_WhenNoCharactersReturned()
    {
        var serviceMock = new Mock<IExternalCharacterService>();
        serviceMock.Setup(s => s.GetCharacterAsync(4)).ReturnsAsync((List<Character>)null!);

        var scheduler = new BattleScheduler(serviceMock.Object);

        var ex = await Assert.ThrowsAsync<Exception>(() => scheduler.ScheduleBattlesAsync(4));
        Assert.Equal("No se recibieron personajes desde la API externa.", ex.Message);
    }

    [Fact]
    public async Task ScheduleBattles_ReturnsBattlesWithSameDate_EveryTwo()
    {
        var characters = Enumerable.Range(1, 6)
            .Select(i => new Character { Id = i, Name = $"Fighter{i}" })
            .ToList();

        var serviceMock = new Mock<IExternalCharacterService>();
        serviceMock.Setup(s => s.GetCharacterAsync(6)).ReturnsAsync(characters);

        var scheduler = new BattleScheduler(serviceMock.Object);
        var result = await scheduler.ScheduleBattlesAsync(6);

        Assert.Equal(result[0].Fecha, result[1].Fecha);

        if (result.Count >= 4)
            Assert.Equal(result[2].Fecha, result[3].Fecha); 

        if (result.Count >= 6)
            Assert.Equal(result[4].Fecha, result[5].Fecha);
    }

}
