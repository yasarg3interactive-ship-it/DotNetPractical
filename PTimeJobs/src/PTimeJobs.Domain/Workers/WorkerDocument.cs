using PTimeJobs.Domain.Users;

namespace PTimeJobs.Domain.Workers;

public sealed class WorkerDocument
{
    private WorkerDocument()
    {
    }

    public Guid DocumentId { get; private set; }
    public Guid WorkerProfileId { get; private set; }
    public string DocumentType { get; private set; } = string.Empty;
    public string DocumentUrl { get; private set; } = string.Empty;
    public string? FileName { get; private set; }
    public string? MimeType { get; private set; }
    public VerificationStatus VerificationStatus { get; private set; }
    public Guid? VerifiedBy { get; private set; }
    public DateTimeOffset? VerifiedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static WorkerDocument Create(
        Guid workerProfileId,
        string documentType,
        string documentUrl,
        string? fileName = null,
        string? mimeType = null)
    {
        if (string.IsNullOrWhiteSpace(documentType))
        {
            throw new InvalidOperationException("Document type is required.");
        }

        if (string.IsNullOrWhiteSpace(documentUrl))
        {
            throw new InvalidOperationException("Document URL is required.");
        }

        return new WorkerDocument
        {
            DocumentId = Guid.NewGuid(),
            WorkerProfileId = workerProfileId,
            DocumentType = documentType,
            DocumentUrl = documentUrl,
            FileName = fileName,
            MimeType = mimeType,
            VerificationStatus = VerificationStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Verify(Guid verifiedBy)
    {
        VerificationStatus = VerificationStatus.Verified;
        VerifiedBy = verifiedBy;
        VerifiedAt = DateTimeOffset.UtcNow;
    }

    public void Reject(Guid verifiedBy)
    {
        VerificationStatus = VerificationStatus.Failed;
        VerifiedBy = verifiedBy;
        VerifiedAt = DateTimeOffset.UtcNow;
    }
}
