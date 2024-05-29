﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Presentation.Web.JobScheduling;

using System.Net;
using System.Threading;
using BridgingIT.DevKit.Common;
using BridgingIT.DevKit.Presentation.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;
using IResult = Microsoft.AspNetCore.Http.IResult;

public class JobSchedulingEndpoints(
    ISchedulerFactory schedulerFactory,
    JobSchedulingEndpointsOptions options = null)
    : EndpointsBase
{
    private readonly ISchedulerFactory schedulerFactory = schedulerFactory;
    private readonly JobSchedulingEndpointsOptions options = options ?? new JobSchedulingEndpointsOptions();

    public override void Map(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this.options.GroupPrefix)
            .WithTags(this.options.GroupTag);

        if (this.options.RequireAuthorization)
        {
            group.RequireAuthorization();
        }

        group.MapGet(string.Empty, this.GetJobs)
            //.AllowAnonymous()
            .Produces<IEnumerable<JobModel>>(200)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError);

        group.MapPost("{name}", this.PostJob)
            //.AllowAnonymous()
            .Produces<IEnumerable<JobModel>>((int)HttpStatusCode.Accepted)
            .Produces<IEnumerable<JobModel>>((int)HttpStatusCode.NotFound)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError);
    }

    private async Task<IResult> GetJobs(CancellationToken cancellationToken)
    {
        return Results.Ok(
            await this.GetAllJobs(
                await this.schedulerFactory.GetScheduler(cancellationToken), cancellationToken));
    }

    private async Task<IResult> PostJob(string name, CancellationToken cancellationToken)
    {
        var scheduler = await this.schedulerFactory.GetScheduler(cancellationToken);
        if (!(await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken)).Any(j => j.Name == name))
        {
            return Results.NotFound();
        }

        await scheduler.TriggerJob(new JobKey(name), cancellationToken);
        await Task.Delay(300, cancellationToken);
        var job = (await this.GetAllJobs(scheduler, cancellationToken))?.FirstOrDefault(j => j.Name == name);

        return Results.Accepted(null, job);
    }

    private async Task<IEnumerable<JobModel>> GetAllJobs(IScheduler scheduler, CancellationToken cancellationToken)
    {
        var results = new List<JobModel>();
        var jobGroups = await scheduler.GetJobGroupNames(cancellationToken);
        var triggerGroups = await scheduler.GetTriggerGroupNames(cancellationToken);
        var executingJobs = await scheduler.GetCurrentlyExecutingJobs(cancellationToken);

        foreach (var group in jobGroups.SafeNull())
        {
            var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
            var jobKeys = await scheduler.GetJobKeys(groupMatcher, cancellationToken);

            foreach (var jobKey in jobKeys.SafeNull())
            {
                var job = await scheduler.GetJobDetail(jobKey, cancellationToken);
                var triggers = await scheduler.GetTriggersOfJob(jobKey, cancellationToken);

                foreach (var trigger in triggers.SafeNull())
                {
                    var jobInfo = new JobModel
                    {
                        Group = group,
                        Name = jobKey.Name,
                        Description = $"{job.Description} ({trigger.Description})",
                        Type = job.JobType.FullName,
                        TriggerName = trigger.Key.Name,
                        TriggerGroup = trigger.Key.Group,
                        TriggerType = trigger.GetType().Name,
                        TriggerState = (await scheduler.GetTriggerState(trigger.Key, cancellationToken)).ToString(),
                        NextFireTime = trigger.GetNextFireTimeUtc(),
                        PreviousFireTime = trigger.GetPreviousFireTimeUtc(),
                        CurrentlyExecuting = executingJobs.SafeWhere(j => j.JobDetail.Key.Name == job.Key.Name).SafeAny()
                    };
                    results.Add(jobInfo);
                }
            }
        }

        return results;
    }
}