using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json.Serialization;

using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services;

public class ExternalCharacterService : IExternalCharacterService
{
    private readonly HttpClient _client;

    public ExternalCharacterService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Character>> GetCharacterAsync(int count)
    {
        string url = $"https://dragonball-api.com/api/characters?page=1&limit={count}";
        var response = await _client.GetFromJsonAsync<CharacterApiResponse>(url);

        if (response?.Items == null || !response.Items.Any())
        {
            throw new Exception("No se pudieron obtener personajes desde la API externa.");
        }

        return response.Items.Select(c => new Character
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    }

    private class CharacterApiResponse
    {
        [JsonPropertyName("items")]
        public List<CharacterItem> Items { get; set; } = new();
    }

    private class CharacterItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

    }
}
