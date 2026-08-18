using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Accommodation;

public sealed class PropertiesService(ApplicationDbContext dbContext) : IPropertiesService
{
    public async Task<PropertyResponse?> GetByIdAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        var property = await dbContext.Properties.AsNoTracking().FirstOrDefaultAsync(p => p.PropertyId == propertyId, cancellationToken);
        return property is null ? null : await BuildResponseAsync(property, cancellationToken);
    }

    public async Task<IReadOnlyCollection<PropertyResponse>> GetByProviderAsync(Guid accommodationProviderId, CancellationToken cancellationToken = default)
    {
        var properties = await dbContext.Properties
            .AsNoTracking()
            .Where(p => p.AccommodationProviderId == accommodationProviderId)
            .ToListAsync(cancellationToken);

        var responses = new List<PropertyResponse>();
        foreach (var property in properties)
        {
            responses.Add(await BuildResponseAsync(property, cancellationToken));
        }

        return responses;
    }

    public async Task<PropertyResponse> CreateAsync(CreatePropertyRequest request, CancellationToken cancellationToken = default)
    {
        var providerExists = await dbContext.AccommodationProviders
            .AsNoTracking()
            .AnyAsync(provider => provider.AccommodationProviderId == request.AccommodationProviderId, cancellationToken);

        if (!providerExists)
        {
            throw new InvalidOperationException("Accommodation provider not found.");
        }

        var property = Property.Create(
            request.AccommodationProviderId,
            request.PropertyName,
            request.PropertyType,
            request.LocationId,
            request.Latitude,
            request.Longitude,
            request.AddressText,
            request.Description);

        dbContext.Properties.Add(property);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await BuildResponseAsync(property, cancellationToken);
    }

    public async Task<PropertyResponse?> DeactivateAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        var property = await dbContext.Properties.FirstOrDefaultAsync(p => p.PropertyId == propertyId, cancellationToken);
        if (property is null)
        {
            return null;
        }

        property.Deactivate();
        await dbContext.SaveChangesAsync(cancellationToken);
        return await BuildResponseAsync(property, cancellationToken);
    }

    public async Task<PropertyResponse?> AddImageAsync(Guid propertyId, AddPropertyImageRequest request, CancellationToken cancellationToken = default)
    {
        var property = await dbContext.Properties.AsNoTracking().FirstOrDefaultAsync(p => p.PropertyId == propertyId, cancellationToken);
        if (property is null)
        {
            return null;
        }

        var image = PropertyImage.Create(propertyId, request.ImageUrl, request.SortOrder, request.IsPrimary);
        dbContext.PropertyImages.Add(image);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await BuildResponseAsync(property, cancellationToken);
    }

    public async Task<PropertyResponse?> AddFacilityAsync(Guid propertyId, AddPropertyFacilityRequest request, CancellationToken cancellationToken = default)
    {
        var property = await dbContext.Properties.AsNoTracking().FirstOrDefaultAsync(p => p.PropertyId == propertyId, cancellationToken);
        if (property is null)
        {
            return null;
        }

        var facilityExists = await dbContext.Facilities.AsNoTracking().AnyAsync(f => f.FacilityId == request.FacilityId, cancellationToken);
        if (!facilityExists)
        {
            throw new InvalidOperationException("Facility not found.");
        }

        var alreadyAdded = await dbContext.PropertyFacilities
            .AsNoTracking()
            .AnyAsync(pf => pf.PropertyId == propertyId && pf.FacilityId == request.FacilityId, cancellationToken);

        if (!alreadyAdded)
        {
            dbContext.PropertyFacilities.Add(PropertyFacility.Create(propertyId, request.FacilityId, request.Details));
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return await BuildResponseAsync(property, cancellationToken);
    }

    private async Task<PropertyResponse> BuildResponseAsync(Property property, CancellationToken cancellationToken)
    {
        var images = await dbContext.PropertyImages
            .AsNoTracking()
            .Where(image => image.PropertyId == property.PropertyId)
            .OrderBy(image => image.SortOrder)
            .Select(image => new PropertyImageResponse(image.PropertyImageId, image.ImageUrl, image.SortOrder, image.IsPrimary))
            .ToListAsync(cancellationToken);

        var facilities = await (
            from propertyFacility in dbContext.PropertyFacilities.AsNoTracking()
            where propertyFacility.PropertyId == property.PropertyId
            join facility in dbContext.Facilities.AsNoTracking() on propertyFacility.FacilityId equals facility.FacilityId
            select new PropertyFacilityResponse(facility.FacilityId, facility.FacilityName, propertyFacility.Details))
            .ToListAsync(cancellationToken);

        return new PropertyResponse(
            property.PropertyId,
            property.AccommodationProviderId,
            property.PropertyName,
            property.PropertyType,
            property.Description,
            property.LocationId,
            property.Latitude,
            property.Longitude,
            property.AddressText,
            property.IsActive,
            property.AverageRating,
            property.RatingCount,
            images,
            facilities);
    }
}
