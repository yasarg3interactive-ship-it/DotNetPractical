using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Accommodation;

public sealed class AccommodationProvidersService(ApplicationDbContext dbContext) : IAccommodationProvidersService
{
    public async Task<AccommodationProviderResponse?> GetByIdAsync(Guid accommodationProviderId, CancellationToken cancellationToken = default)
    {
        var provider = await dbContext.AccommodationProviders
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.AccommodationProviderId == accommodationProviderId, cancellationToken);

        return provider is null ? null : ToResponse(provider);
    }

    public async Task<IReadOnlyCollection<AccommodationProviderResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var providers = await dbContext.AccommodationProviders.AsNoTracking().OrderBy(p => p.BusinessName).ToListAsync(cancellationToken);
        return providers.Select(ToResponse).ToList();
    }

    public async Task<AccommodationProviderResponse> CreateAsync(CreateAccommodationProviderRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var provider = AccommodationProvider.Create(request.UserId, request.BusinessName, request.ContactNumber);
        dbContext.AccommodationProviders.Add(provider);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(provider);
    }

    public async Task<AccommodationProviderResponse?> MarkVerifiedAsync(Guid accommodationProviderId, CancellationToken cancellationToken = default)
    {
        var provider = await dbContext.AccommodationProviders
            .FirstOrDefaultAsync(p => p.AccommodationProviderId == accommodationProviderId, cancellationToken);

        if (provider is null)
        {
            return null;
        }

        provider.MarkVerified();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(provider);
    }

    private static AccommodationProviderResponse ToResponse(AccommodationProvider provider) => new(
        provider.AccommodationProviderId,
        provider.UserId,
        provider.BusinessName,
        provider.VerificationStatus.ToString(),
        provider.ContactNumber,
        provider.CreatedAt);
}
