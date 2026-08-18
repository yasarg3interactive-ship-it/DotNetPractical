namespace PTimeJobs.Domain.Jobs;

public sealed class JobLocation
{
    private JobLocation()
    {
    }

    public Guid JobLocationId { get; private set; }
    public Guid JobId { get; private set; }
    public Guid? LocationId { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public bool IsRemoteAllowed { get; private set; }

    public static JobLocation Create(
        Guid jobId,
        Guid? locationId,
        decimal? latitude = null,
        decimal? longitude = null,
        bool isRemoteAllowed = false)
    {
        return new JobLocation
        {
            JobLocationId = Guid.NewGuid(),
            JobId = jobId,
            LocationId = locationId,
            Latitude = latitude,
            Longitude = longitude,
            IsRemoteAllowed = isRemoteAllowed
        };
    }
}
