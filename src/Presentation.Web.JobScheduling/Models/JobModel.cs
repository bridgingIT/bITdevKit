﻿namespace BridgingIT.DevKit.Presentation.Web.JobScheduling;

public class JobModel
{
    public string Group { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Description { get; set; }

    public string TriggerName { get; set; }

    public string TriggerGroup { get; set; }

    public string TriggerType { get; set; }

    public string TriggerState { get; set; }

    public DateTimeOffset? NextFireTime { get; set; }

    public DateTimeOffset? PreviousFireTime { get; set; }

    public bool CurrentlyExecuting { get; set; }
}