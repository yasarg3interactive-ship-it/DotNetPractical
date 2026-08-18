namespace PTimeJobs.Application.Employers.Dtos;

public sealed record EmployerProfileResponse(
    Guid EmployerProfileId,
    Guid UserId,
    string CompanyName,
    string? BusinessType,
    string? RegistrationNumber,
    string VerificationStatus,
    Guid? LocationId,
    decimal AverageRating,
    int RatingCount,
    DateTimeOffset CreatedAt);

public sealed record CreateEmployerProfileRequest(
    Guid UserId,
    string CompanyName,
    string? BusinessType,
    string? RegistrationNumber,
    Guid? LocationId);
