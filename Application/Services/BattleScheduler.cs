using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class BattleScheduler : IBattleScheduler
{
    private readonly IExternalCharacterService _externalService;
    private readonly Random _random = new();
    private readonly ILoggerService _logger;

    public BattleScheduler(IExternalCharacterService externalService, ILoggerService logger)
    {
        _externalService = externalService;
        _logger = logger;
    }

    public async Task<List<BattleDto>> ScheduleBattlesAsync(int numeroParticipantes)
    {

        _logger.LogInfo($"Iniciando agendamiento con {numeroParticipantes} peleadores");
        if (numeroParticipantes < 2 || numeroParticipantes > 16 || numeroParticipantes % 2 != 0)
            throw new ArgumentException("El número de peleadores debe ser par y entre 2 y 16.");

        var fighters = await _externalService.GetCharacterAsync(numeroParticipantes);

        if (fighters == null || !fighters.Any())
            throw new Exception("No se recibieron personajes desde la API externa.");

        var names = fighters.Select(f => f.Name).ToList();
        _logger.LogInfo($"Nombres de peleadores obtenidos: {string.Join(", ", names)}");
        var schedules = new List<BattleDto>();
        DateTime startDate = DateTime.UtcNow.Date.AddDays(30);

        names = names.OrderBy(_ => _random.Next()).ToList();

        int day = 0;
        for (int i = 0; i < names.Count; i += 2)
        {
            var date = startDate.AddDays(day / 2);
            var battle = new BattleDto(names[i], names[i + 1], date);
            schedules.Add(battle);
            day++;
        }

        return schedules;
    }
}
