using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class EditActivity
{
    public class Command : IRequest
    {
        public required Activity Activity;
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .FindAsync([request.Activity.Id], cancellationToken)
                    ?? throw new Exception("Activity not found");

            activity.Category = request.Activity.Category;
            activity.Title = request.Activity.Title;
            activity.Date = request.Activity.Date;
            activity.Description = request.Activity.Description;
            activity.IsCanceled = request.Activity.IsCanceled;
            activity.Latitude = request.Activity.Latitude;
            activity.Longitude = request.Activity.Longitude;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
