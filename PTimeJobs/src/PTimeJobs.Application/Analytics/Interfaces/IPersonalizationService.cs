using PTimeJobs.Application.Analytics.Dtos;

namespace PTimeJobs.Application.Analytics.Interfaces;

public interface IPersonalizationService
{
    Task<SearchHistoryResponse> RecordSearchAsync(CreateSearchHistoryRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<SearchHistoryResponse>> GetSearchHistoryByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<RecommendationHistoryResponse> CreateRecommendationAsync(CreateRecommendationRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<RecommendationHistoryResponse>> GetRecommendationsByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<RecommendationHistoryResponse?> RecordClickAsync(Guid recommendationId, CancellationToken cancellationToken = default);

    Task<RecommendationHistoryResponse?> RecordDismissalAsync(Guid recommendationId, CancellationToken cancellationToken = default);

    Task<RecommendationHistoryResponse?> RecordConversionAsync(Guid recommendationId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UserPreferenceResponse>> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserPreferenceResponse> UpsertPreferenceAsync(Guid userId, UpsertUserPreferenceRequest request, CancellationToken cancellationToken = default);
}
