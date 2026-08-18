using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Jobs;

public sealed class MatchingScoresService(ApplicationDbContext dbContext) : IMatchingScoresService
{
    public async Task<IReadOnlyCollection<MatchingScoreResponse>> GetByJobAsync(Guid jobId, int top, CancellationToken cancellationToken = default)
    {
        top = top is < 1 or > 200 ? 20 : top;

        var scores = await dbContext.MatchingScores
            .AsNoTracking()
            .Where(score => score.JobId == jobId)
            .OrderByDescending(score => score.OverallScore)
            .Take(top)
            .ToListAsync(cancellationToken);

        return scores.Select(ToResponse).ToList();
    }

    public async Task<IReadOnlyCollection<MatchingScoreResponse>> GetByWorkerAsync(Guid workerProfileId, int top, CancellationToken cancellationToken = default)
    {
        top = top is < 1 or > 200 ? 20 : top;

        var scores = await dbContext.MatchingScores
            .AsNoTracking()
            .Where(score => score.WorkerProfileId == workerProfileId)
            .OrderByDescending(score => score.OverallScore)
            .Take(top)
            .ToListAsync(cancellationToken);

        return scores.Select(ToResponse).ToList();
    }

    public async Task<MatchingScoreResponse> CreateAsync(CreateMatchingScoreRequest request, CancellationToken cancellationToken = default)
    {
        var jobExists = await dbContext.Jobs.AsNoTracking().AnyAsync(job => job.JobId == request.JobId, cancellationToken);
        if (!jobExists)
        {
            throw new InvalidOperationException("Job not found.");
        }

        var workerExists = await dbContext.WorkerProfiles
            .AsNoTracking()
            .AnyAsync(worker => worker.WorkerProfileId == request.WorkerProfileId, cancellationToken);

        if (!workerExists)
        {
            throw new InvalidOperationException("Worker profile not found.");
        }

        var score = MatchingScore.Create(
            request.WorkerProfileId,
            request.JobId,
            request.ModelVersion,
            request.OverallScore,
            request.SkillScore,
            request.DistanceScore,
            request.AvailabilityScore,
            request.ExperienceScore,
            request.SalaryScore,
            request.RatingScore);

        dbContext.MatchingScores.Add(score);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(score);
    }

    private static MatchingScoreResponse ToResponse(MatchingScore score) => new(
        score.MatchingScoreId,
        score.WorkerProfileId,
        score.JobId,
        score.ModelVersion,
        score.OverallScore,
        score.SkillScore,
        score.DistanceScore,
        score.AvailabilityScore,
        score.ExperienceScore,
        score.SalaryScore,
        score.RatingScore,
        score.CalculatedAt);
}
