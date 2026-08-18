namespace PTimeJobs.Domain.Accommodation;

public sealed class Facility
{
    private Facility()
    {
    }

    public Guid FacilityId { get; private set; }
    public string FacilityName { get; private set; } = string.Empty;
    public string? FacilityCategory { get; private set; }

    public static Facility Create(string facilityName, string? facilityCategory = null)
    {
        if (string.IsNullOrWhiteSpace(facilityName))
        {
            throw new InvalidOperationException("Facility name is required.");
        }

        return new Facility
        {
            FacilityId = Guid.NewGuid(),
            FacilityName = facilityName,
            FacilityCategory = facilityCategory
        };
    }
}
