namespace PTimeJobs.Domain.Jobs;

public enum ApplicationStatus
{
    Submitted = 0,
    Reviewing = 1,
    Shortlisted = 2,
    Interview = 3,
    Offered = 4,
    Hired = 5,
    Rejected = 6,
    Withdrawn = 7
}
