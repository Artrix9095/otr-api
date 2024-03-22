using API.DTOs;
using API.Entities;
using API.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ApiVersion(1)]
[EnableCors]
[Route("api/v{version:apiVersion}/[controller]")]
public class ClientsController(IOAuthClientService clientService) : Controller
{
    [HttpPatch("{id:int}/ratelimit")]
    [Authorize(Roles = "admin")]
    [EndpointSummary("Patches the ratelimit for a given client")]
    public async Task<Results<BadRequest, NotFound<string>, Ok<OAuthClientDTO>>> PatchRatelimitAsync(int id, [FromBody] JsonPatchDocument<RateLimitOverrides> patchedOverrides)
    {
        OAuthClientDTO? currentClient = await clientService.GetAsync(id);

        if (currentClient == null)
        {
            return TypedResults.NotFound("Client not found");
        }

        RateLimitOverrides overrides = currentClient.RateLimitOverrides;

        if (patchedOverrides.Operations.Any(op => op.op != "replace"))
        {
            return TypedResults.BadRequest();
        }

        patchedOverrides.ApplyTo(overrides, ModelState);

        // We know client is not null here due to the previous null check
        OAuthClientDTO? client = await clientService.SetRatelimitOverridesAsync(id, overrides);
        return TypedResults.Ok(client!);
    }
}
