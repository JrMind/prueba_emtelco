using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BattlesController : ControllerBase
{
    private readonly IBattleScheduler _scheduler;

    public BattlesController(IBattleScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] int fighters)
    {
        try
        {
            var result = await _scheduler.ScheduleBattlesAsync(fighters);
            return Ok(new { batallas = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

}
