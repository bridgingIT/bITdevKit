// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Application.JobScheduling;

using BridgingIT.DevKit.Common;
using EnsureThat;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

[DisallowConcurrentExecution]
[PersistJobDataAfterExecution]
public abstract partial class JobBase : IJob
{
    private const string JobIdKey = "JobId";

    protected JobBase(ILoggerFactory loggerFactory)
    {
        EnsureArg.IsNotNull(loggerFactory, nameof(loggerFactory));

        this.Logger = loggerFactory.CreateLogger(this.GetType());
    }

    public ILogger Logger { get; }

    public DateTime LastProcessed { get; set; }

    public long LastDuration { get; set; }

    public JobStatus LastStatus { get; set; }

    public string LastErrorMessage { get; set; }

    public virtual async Task Execute(IJobExecutionContext context)
    {
        EnsureArg.IsNotNull(context, nameof(context));

        var jobId = context.JobDetail.JobDataMap?.GetString(JobIdKey) ?? context.FireInstanceId;
        var jobTypeName = context.JobDetail.JobType.Name;
        var watch = ValueStopwatch.StartNew();
        long elapsedMilliseconds = 0;

        if (context.CancellationToken.IsCancellationRequested)
        {
            this.Logger.LogWarning("{LogKey} processing cancelled (type={JobType}, id={JobId})", Constants.LogKey, jobTypeName, jobId);
            context.CancellationToken.ThrowIfCancellationRequested();
        }
        else
        {
            TypedLogger.LogProcessing(this.Logger, Constants.LogKey, jobTypeName, jobId);

            GetJobProperties(context);

            try
            {
                await this.Process(context, context.CancellationToken).AnyContext();
            }
            catch (Exception ex)
            {
                PutJobProperties(context, JobStatus.Fail, ex.Message, watch.GetElapsedMilliseconds());

                throw;
            }
            finally
            {
                elapsedMilliseconds = watch.GetElapsedMilliseconds();
            }

            PutJobProperties(context, JobStatus.Success, null, elapsedMilliseconds);
        }

        TypedLogger.LogProcessed(this.Logger, Constants.LogKey, jobTypeName, jobId, elapsedMilliseconds);

        void GetJobProperties(IJobExecutionContext context)
        {
            if (context.JobDetail.JobDataMap.TryGetString(nameof(this.LastStatus), out var lastStatus))
            {
                Enum.TryParse(lastStatus, out JobStatus status);
                this.LastStatus = status;
            }

            if (context.JobDetail.JobDataMap.TryGetString(nameof(this.LastErrorMessage), out var lastErrorMessage))
            {
                this.LastErrorMessage = lastErrorMessage;
            }

            if (context.JobDetail.JobDataMap.TryGetDateTime(nameof(this.LastProcessed), out var lastProcessed))
            {
                this.LastProcessed = lastProcessed;
            }

            if (context.JobDetail.JobDataMap.TryGetLong(nameof(this.LastDuration), out var lastDuration))
            {
                this.LastDuration = lastDuration;
            }
        }

        void PutJobProperties(IJobExecutionContext context, JobStatus status, string errorMessage, long elapsedMilliseconds)
        {
            this.LastStatus = status;
            this.LastErrorMessage = errorMessage;
            this.LastProcessed = DateTime.UtcNow;
            this.LastDuration = elapsedMilliseconds;

            context.JobDetail.JobDataMap.Put(nameof(this.LastStatus), this.LastStatus.ToString());
            context.JobDetail.JobDataMap.Put(nameof(this.LastErrorMessage), this.LastErrorMessage);
            context.JobDetail.JobDataMap.Put(nameof(this.LastProcessed), this.LastProcessed);
            context.JobDetail.JobDataMap.Put(nameof(this.LastDuration), this.LastDuration);
        }
    }

    public abstract Task Process(IJobExecutionContext context, CancellationToken cancellationToken = default);

    public static partial class TypedLogger
    {
        [LoggerMessage(0, LogLevel.Information, "{LogKey} processing (type={JobType}, id={JobId})")]
        public static partial void LogProcessing(ILogger logger, string logKey, string jobType, string jobId);

        [LoggerMessage(1, LogLevel.Information, "{LogKey} processed (type={JobType}, id={JobId}) -> took {TimeElapsed:0.0000} ms")]
        public static partial void LogProcessed(ILogger logger, string logKey, string jobType, string jobId, long timeElapsed);
    }
}

public enum JobStatus
{
    Unknown = 0,
    Success = 1,
    Fail = 2
}