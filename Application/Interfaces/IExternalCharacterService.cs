using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;


namespace Application.Interfaces;

public interface IExternalCharacterService
{
    Task<List<Character>> GetCharacterAsync(int count);
}