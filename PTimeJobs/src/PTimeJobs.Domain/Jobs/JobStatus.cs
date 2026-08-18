namespace PTimeJobs.Domain.Jobs;

public enum JobStatus
{
    Draft = 0,
    Open = 1,
    Paused = 2,
    Closed = 3,
    Filled = 4,
    Cancelled = 5
}
