using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Application.DTOs;

namespace Application.Interfaces;

public interface IBattleScheduler
{
    Task<List<BattleDto>> ScheduleBattlesAsync(int numeroParticipantes);
}