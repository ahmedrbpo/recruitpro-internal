using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Recruitment.Dtos;
using RecruitPro.Application.Recruitment.Skills.CreateSkill;
using RecruitPro.Application.Recruitment.Skills.GetSkillById;

namespace RecruitPro.Api.Controllers;

public sealed record CreateSkillRequest(string Name, string? Category, string? Description);

[Route("api/v1/skills")]
public sealed class SkillsController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Recruitment.Skill.Create")]
    public async Task<ActionResult<ApiResponse<SkillDto>>> Create(CreateSkillRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateSkillCommand(request.Name, request.Category, request.Description), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("Recruitment.Skill.View")]
    public async Task<ActionResult<ApiResponse<SkillDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSkillByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }
}
