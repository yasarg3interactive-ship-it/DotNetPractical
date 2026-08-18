using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class SkillsController(ISkillsService skillsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<SkillResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? search, CancellationToken cancellationToken)
    {
        var skills = await skillsService.GetAllAsync(search, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<SkillResponse>>.Success(skills));
    }

    [HttpGet("{skillId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SkillResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<SkillResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid skillId, CancellationToken cancellationToken)
    {
        var skill = await skillsService.GetByIdAsync(skillId, cancellationToken);

        if (skill is null)
        {
            return NotFound(ApiResponse<SkillResponse>.Failure("Skill not found."));
        }

        return Ok(ApiResponse<SkillResponse>.Success(skill));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SkillResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSkillRequest request, CancellationToken cancellationToken)
    {
        var skill = await skillsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { skillId = skill.SkillId },
            ApiResponse<SkillResponse>.Success(skill, "Skill created."));
    }
}
