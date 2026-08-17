namespace PTimeJobs.Domain.Users;

public enum VerificationStatus
{
    Pending = 0,
    Verified = 1,
    Failed = 2,
    Expired = 3,
    Revoked = 4
}
