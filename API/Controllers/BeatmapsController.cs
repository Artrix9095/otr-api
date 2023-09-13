using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeatmapsController : Controller
{
	private readonly IBeatmapService _beatmapService;
	private readonly ILogger<BeatmapsController> _logger;

	public BeatmapsController(ILogger<BeatmapsController> logger, IBeatmapService beatmapService)
	{
		_logger = logger;
		_beatmapService = beatmapService;
	}

	[HttpGet("all")]
	public async Task<ActionResult<IEnumerable<Beatmap>>> GetAllAsync() => Ok(await _beatmapService.GetAllAsync());

	[HttpGet("{beatmapId:long}")]
	public async Task<ActionResult<Beatmap>> GetByOsuBeatmapIdAsync(long beatmapId)
	{
		var beatmap = await _beatmapService.GetBeatmapDTOByBeatmapIdAsync(beatmapId);
		if (beatmap == null)
		{
			return NotFound("No matching beatmapId in the database.");
		}

		return Ok(beatmap);
	}
}