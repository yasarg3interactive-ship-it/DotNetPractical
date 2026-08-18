using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Complaints.Dtos;

namespace PTimeJobs.Application.Complaints.Interfaces;

public interface IComplaintsService
{
    Task<ComplaintResponse?> GetByIdAsync(Guid complaintId, CancellationToken cancellationToken = default);

    Task<PagedResult<ComplaintResponse>> SearchAsync(string? status, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<ComplaintResponse> CreateAsync(CreateComplaintRequest request, CancellationToken cancellationToken = default);

    Task<ComplaintResponse?> AssignAsync(Guid complaintId, Guid assignedTo, CancellationToken cancellationToken = default);

    Task<ComplaintResponse?> ResolveAsync(Guid complaintId, ResolveComplaintRequest request, CancellationToken cancellationToken = default);

    Task<ComplaintResponse?> RejectAsync(Guid complaintId, ResolveComplaintRequest request, CancellationToken cancellationToken = default);

    Task<ComplaintResponse?> EscalateAsync(Guid complaintId, CancellationToken cancellationToken = default);
}
