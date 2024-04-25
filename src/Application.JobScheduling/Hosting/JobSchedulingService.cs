﻿// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Application.JobScheduling;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BridgingIT.DevKit.Common;
using EnsureThat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Quartz;
using Quartz.Spi;

public class JobSchedulingService : BackgroundService
{
    private readonly ILogger<JobSchedulingService> logger;
    private readonly ISchedulerFactory schedulerFactory;
    private readonly IJobFactory jobFactory;
    private readonly IEnumerable<JobSchedule> jobSchedules;
    private readonly JobSchedulingOptions options;

    public JobSchedulingService(
        ILoggerFactory loggerFactory,
        ISchedulerFactory schedulerFactory,
        IJobFactory jobFactory,
        IEnumerable<JobSchedule> jobSchedules = null,
        JobSchedulingOptions options = null)
    {
        EnsureArg.IsNotNull(schedulerFactory, nameof(schedulerFactory));
        EnsureArg.IsNotNull(jobFactory, nameof(jobFactory));

        this.logger = loggerFactory?.CreateLogger<JobSchedulingService>() ?? NullLoggerFactory.Instance.CreateLogger<JobSchedulingService>();
        this.schedulerFactory = schedulerFactory;
        this.jobFactory = jobFactory;
        this.jobSchedules = jobSchedules;
        this.options = options ?? new JobSchedulingOptions();
    }

    public IScheduler Scheduler { get; set; }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (this.Scheduler?.IsShutdown == false)
        {
            this.logger.LogInformation("{LogKey} scheduling service stopping", Constants.LogKey);
            await (this.Scheduler?.Shutdown(cancellationToken)).AnyContext();
            this.logger.LogInformation("{LogKey} scheduling service stopped", Constants.LogKey);
        }

        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!this.options.Enabled)
        {
            return;
        }

        if (this.options.StartupDelay.TotalMilliseconds > 0)
        {
            this.logger.LogDebug("{LogKey} scheduling service startup delayed", Constants.LogKey);

            await Task.Delay(this.options.StartupDelay, cancellationToken);
        }

        try
        {
            this.logger.LogInformation("{LogKey} scheduling service starting", Constants.LogKey);
            this.Scheduler = await this.schedulerFactory.GetScheduler(cancellationToken).AnyContext();
            this.Scheduler.JobFactory = this.jobFactory;

            foreach (var jobSchedule in this.jobSchedules.SafeNull())
            {
                var jobDetail = CreateJobDetail(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                try
                {
                    await this.Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).AnyContext();
                }
                catch (ObjectAlreadyExistsException ex)
                {
                    this.logger.LogError(ex, "{LogKey} schedule job failed: {ErrorMessage} (type={JobType})", Constants.LogKey, ex.Message, jobDetail.JobType);
                }
            }

            await this.Scheduler.Start(cancellationToken).AnyContext();
            this.logger.LogInformation("{LogKey} scheduling service started", Constants.LogKey);
        }
        catch (SchedulerException ex)
        {
            this.logger.LogError(ex, "{LogKey} scheduling service failed: {ErrorMessage}", Constants.LogKey, ex.Message);
        }
    }

    private static ITrigger CreateTrigger(JobSchedule schedule)
    {
        return TriggerBuilder
            .Create()
            .WithIdentity($"{schedule.JobType.FullName}.trigger")
            .WithCronSchedule(schedule.CronExpression)
            .WithDescription(schedule.CronExpression)
            .Build();
    }

    private static IJobDetail CreateJobDetail(JobSchedule schedule)
    {
        return JobBuilder
            .Create(schedule.JobType)
            .WithIdentity(schedule.JobType.FullName)
            .UsingJobData("JobId", GuidGenerator.CreateSequential().ToString("N"))
            .WithDescription(schedule.JobType.Name)
            .Build();
    }
}
