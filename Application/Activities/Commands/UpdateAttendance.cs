using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Commands;

public class UpdateAtendance
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .Include(x => x.Attendees)
                .ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (activity == null) return Result<Unit>.Failiure("Activity not found", 404);

            var userId = userAccessor.GetUserId();
            var attendance = activity.Attendees.FirstOrDefault(x => x.UserId == userId);
            var isHost = activity.Attendees.Any(x => x.IsHost && x.UserId == userId);

            if (attendance != null)
            {
                if (isHost) activity.IsCanceled = !activity.IsCanceled;
                else activity.Attendees.Remove(attendance);
            }
            else
            {
                activity.Attendees.Add(new ActivityAttendee
                {
                    ActivityId = activity.Id,
                    UserId = userId,
                    IsHost = false
                });
            }

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failiure("Problem updating Db", 400);
        }
    }
}
