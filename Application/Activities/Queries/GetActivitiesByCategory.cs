using System;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivitiesByCategory
{
    public class Query : IRequest<List<Activity>>
    {
        public required string Category { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Query, List<Activity>>
    {
        public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Activities.Where(act => act.Category == request.Category).ToListAsync(cancellationToken);
        }
    }
}
