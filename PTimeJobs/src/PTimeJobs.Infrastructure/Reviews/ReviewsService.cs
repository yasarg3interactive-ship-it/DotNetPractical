using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Reviews.Dtos;
using PTimeJobs.Application.Reviews.Interfaces;
using PTimeJobs.Domain.Reviews;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Reviews;

public sealed class ReviewsService(ApplicationDbContext dbContext) : IReviewsService
{
    public async Task<ReviewResponse?> GetByIdAsync(Guid reviewId, CancellationToken cancellationToken = default)
    {
        var review = await dbContext.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.ReviewId == reviewId, cancellationToken);
        return review is null ? null : ToResponse(review);
    }

    public async Task<PagedResult<ReviewResponse>> GetForEntityAsync(
        string targetEntityType,
        Guid targetEntityId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query = dbContext.Reviews
            .AsNoTracking()
            .Where(r => r.TargetEntityType == targetEntityType && r.TargetEntityId == targetEntityId && r.Status == ReviewStatus.Published);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ReviewResponse>(items.Select(ToResponse).ToList(), page, pageSize, totalCount);
    }

    public async Task<ReviewResponse> CreateAsync(CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var reviewerExists = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.UserId == request.ReviewerUserId, cancellationToken);

        if (!reviewerExists)
        {
            throw new InvalidOperationException("Reviewer user not found.");
        }

        var review = Review.Create(
            request.ReviewerUserId,
            request.TargetEntityType,
            request.TargetEntityId,
            request.Rating,
            request.ReviewText,
            request.RelatedEntityType,
            request.RelatedEntityId);

        dbContext.Reviews.Add(review);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(review);
    }

    public async Task<ReviewResponse?> FlagAsync(Guid reviewId, CancellationToken cancellationToken = default)
    {
        var review = await dbContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId, cancellationToken);
        if (review is null)
        {
            return null;
        }

        review.Flag();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(review);
    }

    public async Task<ReviewResponse?> HideAsync(Guid reviewId, CancellationToken cancellationToken = default)
    {
        var review = await dbContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId, cancellationToken);
        if (review is null)
        {
            return null;
        }

        review.Hide();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(review);
    }

    private static ReviewResponse ToResponse(Review review) => new(
        review.ReviewId,
        review.ReviewerUserId,
        review.TargetEntityType,
        review.TargetEntityId,
        review.RelatedEntityType,
        review.RelatedEntityId,
        review.Rating,
        review.ReviewText,
        review.Status.ToString(),
        review.CreatedAt,
        review.UpdatedAt);
}
