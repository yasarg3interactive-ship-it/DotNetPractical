namespace PTimeJobs.Application.Jobs.Dtos;

public sealed record JobLocationResponse(Guid JobLocationId, Guid JobId, Guid? LocationId, decimal? Latitude, decimal? Longitude, bool IsRemoteAllowed);

public sealed record AddJobLocationRequest(Guid? LocationId, decimal? Latitude, decimal? Longitude, bool IsRemoteAllowed);

public sealed record JobScheduleResponse(
    Guid JobScheduleId,
    Guid JobId,
    short? DayOfWeek,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    DateOnly? StartDate,
    DateOnly? EndDate,
    string? ShiftLabel,
    int RequiredWorkers);

public sealed record AddJobScheduleRequest(
    short? DayOfWeek,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    DateOnly? StartDate,
    DateOnly? EndDate,
    string? ShiftLabel,
    int RequiredWorkers);

public sealed record JobSkillResponse(Guid JobId, Guid SkillId, string SkillName, short RequiredLevel, bool IsMandatory);

public sealed record AddJobSkillRequest(Guid SkillId, short RequiredLevel, bool IsMandatory);

public sealed record ShortlistResponse(Guid ShortlistId, Guid ApplicationId, Guid? ShortlistedBy, string? Notes, DateTimeOffset CreatedAt);

public sealed record AddShortlistRequest(Guid? ShortlistedBy, string? Notes);

public sealed record HiringStatusHistoryResponse(
    Guid HiringStatusHistoryId,
    Guid ApplicationId,
    string? OldStatus,
    string NewStatus,
    Guid? ChangedBy,
    string? Reason,
    DateTimeOffset CreatedAt);

public sealed record MatchingScoreResponse(
    Guid MatchingScoreId,
    Guid WorkerProfileId,
    Guid JobId,
    string ModelVersion,
    decimal OverallScore,
    decimal? SkillScore,
    decimal? DistanceScore,
    decimal? AvailabilityScore,
    decimal? ExperienceScore,
    decimal? SalaryScore,
    decimal? RatingScore,
    DateTimeOffset CalculatedAt);

public sealed record CreateMatchingScoreRequest(
    Guid WorkerProfileId,
    Guid JobId,
    string ModelVersion,
    decimal OverallScore,
    decimal? SkillScore,
    decimal? DistanceScore,
    decimal? AvailabilityScore,
    decimal? ExperienceScore,
    decimal? SalaryScore,
    decimal? RatingScore);
