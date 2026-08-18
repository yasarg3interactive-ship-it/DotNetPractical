using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Food.Dtos;
using PTimeJobs.Application.Food.Interfaces;
using PTimeJobs.Domain.Food;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Food;

public sealed class FoodProvidersService(ApplicationDbContext dbContext) : IFoodProvidersService
{
    public async Task<FoodProviderResponse?> GetByIdAsync(Guid foodProviderId, CancellationToken cancellationToken = default)
    {
        var provider = await dbContext.FoodProviders.AsNoTracking().FirstOrDefaultAsync(p => p.FoodProviderId == foodProviderId, cancellationToken);
        return provider is null ? null : ToResponse(provider);
    }

    public async Task<IReadOnlyCollection<FoodProviderResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var providers = await dbContext.FoodProviders.AsNoTracking().OrderBy(p => p.BusinessName).ToListAsync(cancellationToken);
        return providers.Select(ToResponse).ToList();
    }

    public async Task<FoodProviderResponse> CreateAsync(CreateFoodProviderRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var provider = FoodProvider.Create(request.UserId, request.BusinessName, request.ProviderType, request.LocationId);
        dbContext.FoodProviders.Add(provider);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(provider);
    }

    public async Task<FoodProviderResponse?> MarkVerifiedAsync(Guid foodProviderId, CancellationToken cancellationToken = default)
    {
        var provider = await dbContext.FoodProviders.FirstOrDefaultAsync(p => p.FoodProviderId == foodProviderId, cancellationToken);
        if (provider is null)
        {
            return null;
        }

        provider.MarkVerified();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(provider);
    }

    public async Task<IReadOnlyCollection<DeliveryAreaResponse>> GetDeliveryAreasAsync(Guid foodProviderId, CancellationToken cancellationToken = default)
    {
        return await dbContext.DeliveryAreas
            .AsNoTracking()
            .Where(area => area.FoodProviderId == foodProviderId)
            .Select(area => new DeliveryAreaResponse(area.DeliveryAreaId, area.FoodProviderId, area.AreaId, area.RadiusKm, area.DeliveryFee, area.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<DeliveryAreaResponse?> AddDeliveryAreaAsync(
        Guid foodProviderId,
        CreateDeliveryAreaRequest request,
        CancellationToken cancellationToken = default)
    {
        var providerExists = await dbContext.FoodProviders.AsNoTracking().AnyAsync(p => p.FoodProviderId == foodProviderId, cancellationToken);
        if (!providerExists)
        {
            return null;
        }

        var deliveryArea = DeliveryArea.Create(foodProviderId, request.AreaId, request.RadiusKm, request.DeliveryFee);
        dbContext.DeliveryAreas.Add(deliveryArea);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeliveryAreaResponse(
            deliveryArea.DeliveryAreaId,
            deliveryArea.FoodProviderId,
            deliveryArea.AreaId,
            deliveryArea.RadiusKm,
            deliveryArea.DeliveryFee,
            deliveryArea.IsActive);
    }

    private static FoodProviderResponse ToResponse(FoodProvider provider) => new(
        provider.FoodProviderId,
        provider.UserId,
        provider.BusinessName,
        provider.ProviderType,
        provider.VerificationStatus.ToString(),
        provider.LocationId,
        provider.AverageRating,
        provider.RatingCount,
        provider.CreatedAt);
}
