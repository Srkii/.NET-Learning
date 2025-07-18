using Application.Activities.Commands;
using Application.Activities.DTOs;
using Application.Activities.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<ActivityDto>>> GetActivities()
    {
        return await Mediator.Send(new GetActivityList.Query());
    }

    [HttpGet("getById/{id}")]
    public async Task<ActionResult<ActivityDto>> GetActivityById(string id)
    {
        return HandleResult(await Mediator.Send(new GetActivityById.Query { Id = id }));
    }

    [HttpGet("getByCategory/{category}")]
    public async Task<ActionResult<List<Activity>>> GetActivitiesByCategory(string category)
    {
        return await Mediator.Send(new GetActivitiesByCategory.Query { Category = category });
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateActivity(CreateActivityDto activityDto)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto }));
    }

    [HttpPut]
    public async Task<ActionResult<Unit>> EditActivity(EditActivityDto activityDto)
    {
        return HandleResult(await Mediator.Send(new EditActivity.Command { ActivityDto = activityDto }));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Unit>> DeleteActivity(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = id }));
    }

    [HttpPost("{id}/attend")]
    public async Task<ActionResult<Unit>> Attend(string id)
    {
        return HandleResult(await Mediator.Send(new UpdateAtendance.Command { Id = id }));
    }
}