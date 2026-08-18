using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Analytics.Dtos;
using PTimeJobs.Application.Analytics.Interfaces;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Analytics;

public sealed class PersonalizationService(ApplicationDbContext dbContext) : IPersonalizationService
{
    public async Task<SearchHistoryResponse> RecordSearchAsync(CreateSearchHistoryRequest request, CancellationToken cancellationToken = default)
    {
        var search = SearchHistory.Create(request.SearchScope, request.UserId, request.QueryText, request.ResultCount, request.LocationId);
        dbContext.SearchHistories.Add(search);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(search);
    }

    public async Task<IReadOnlyCollection<SearchHistoryResponse>> GetSearchHistoryByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var searches = await dbContext.SearchHistories
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);

        return searches.Select(ToResponse).ToList();
    }

    public async Task<RecommendationHistoryResponse> CreateRecommendationAsync(CreateRecommendationRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var recommendation = RecommendationHistory.Create(
            request.UserId,
            request.RecommendationType,
            request.TargetEntityType,
            request.TargetEntityId,
            request.Score,
            request.ModelVersion);

        dbContext.RecommendationHistories.Add(recommendation);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(recommendation);
    }

    public async Task<IReadOnlyCollection<RecommendationHistoryResponse>> GetRecommendationsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var recommendations = await dbContext.RecommendationHistories
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.ShownAt)
            .ToListAsync(cancellationToken);

        return recommendations.Select(ToResponse).ToList();
    }

    public async Task<RecommendationHistoryResponse?> RecordClickAsync(Guid recommendationId, CancellationToken cancellationToken = default)
    {
        var recommendation = await dbContext.RecommendationHistories
            .FirstOrDefaultAsync(r => r.RecommendationId == recommendationId, cancellationToken);

        if (recommendation is null)
        {
            return null;
        }

        recommendation.RecordClick();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(recommendation);
    }

    public async Task<RecommendationHistoryResponse?> RecordDismissalAsync(Guid recommendationId, CancellationToken cancellationToken = default)
    {
        var recommendation = await dbContext.RecommendationHistories
            .FirstOrDefaultAsync(r => r.RecommendationId == recommendationId, cancellationToken);

        if (recommendation is null)
        {
            return null;
        }

        recommendation.RecordDismissal();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(recommendation);
    }

    public async Task<RecommendationHistoryResponse?> RecordConversionAsync(Guid recommendationId, CancellationToken cancellationToken = default)
    {
        var recommendation = await dbContext.RecommendationHistories
            .FirstOrDefaultAsync(r => r.RecommendationId == recommendationId, cancellationToken);

        if (recommendation is null)
        {
            return null;
        }

        recommendation.RecordConversion();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(recommendation);
    }

    public async Task<IReadOnlyCollection<UserPreferenceResponse>> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var preferences = await dbContext.UserPreferences
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);

        return preferences.Select(ToResponse).ToList();
    }

    public async Task<UserPreferenceResponse> UpsertPreferenceAsync(Guid userId, UpsertUserPreferenceRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == userId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var existing = await dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId && p.PreferenceScope == request.PreferenceScope, cancellationToken);

        if (existing is not null)
        {
            existing.UpdatePreferences(request.Preferences);
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing);
        }

        var preference = UserPreference.Create(userId, request.PreferenceScope, request.Preferences);
        dbContext.UserPreferences.Add(preference);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(preference);
    }

    private static SearchHistoryResponse ToResponse(SearchHistory search) => new(
        search.SearchId,
        search.UserId,
        search.SearchScope,
        search.QueryText,
        search.ResultCount,
        search.CreatedAt);

    private static RecommendationHistoryResponse ToResponse(RecommendationHistory recommendation) => new(
        recommendation.RecommendationId,
        recommendation.UserId,
        recommendation.RecommendationType,
        recommendation.TargetEntityType,
        recommendation.TargetEntityId,
        recommendation.Score,
        recommendation.ShownAt,
        recommendation.ClickedAt,
        recommendation.DismissedAt,
        recommendation.ConvertedAt);

    private static UserPreferenceResponse ToResponse(UserPreference preference) => new(
        preference.PreferenceId,
        preference.UserId,
        preference.PreferenceScope,
        preference.Preferences,
        preference.UpdatedAt);
}
