namespace PTimeJobs.Domain.Accommodation;

public sealed class PropertyFacility
{
    private PropertyFacility()
    {
    }

    public Guid PropertyId { get; private set; }
    public Guid FacilityId { get; private set; }
    public string? Details { get; private set; }

    public static PropertyFacility Create(Guid propertyId, Guid facilityId, string? details = null)
    {
        return new PropertyFacility
        {
            PropertyId = propertyId,
            FacilityId = facilityId,
            Details = details
        };
    }
}
