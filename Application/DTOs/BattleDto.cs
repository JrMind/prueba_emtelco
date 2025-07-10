using System;

namespace Application.DTOs;

public class BattleDto
{
    public string Batalla { get; set; }
    public string Fecha { get; set; }

    public BattleDto(string fighter1, string fighter2, DateTime date)
    {
        Batalla = $"{fighter1} vs {fighter2}";
        Fecha = date.ToString("yyyy/MM/dd");
    }
}
