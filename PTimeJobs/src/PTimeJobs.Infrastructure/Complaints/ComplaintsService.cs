using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Complaints.Dtos;
using PTimeJobs.Application.Complaints.Interfaces;
using PTimeJobs.Domain.Complaints;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Complaints;

public sealed class ComplaintsService(ApplicationDbContext dbContext) : IComplaintsService
{
    public async Task<ComplaintResponse?> GetByIdAsync(Guid complaintId, CancellationToken cancellationToken = default)
    {
        var complaint = await dbContext.Complaints.AsNoTracking().FirstOrDefaultAsync(c => c.ComplaintId == complaintId, cancellationToken);
        return complaint is null ? null : ToResponse(complaint);
    }

    public async Task<PagedResult<ComplaintResponse>> SearchAsync(
        string? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query = dbContext.Complaints.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<ComplaintStatus>(status, true, out var statusValue))
        {
            query = query.Where(c => c.Status == statusValue);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ComplaintResponse>(items.Select(ToResponse).ToList(), page, pageSize, totalCount);
    }

    public async Task<ComplaintResponse> CreateAsync(CreateComplaintRequest request, CancellationToken cancellationToken = default)
    {
        var complainantExists = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.UserId == request.ComplainantUserId, cancellationToken);

        if (!complainantExists)
        {
            throw new InvalidOperationException("Complainant user not found.");
        }

        var complaint = Complaint.Create(
            request.ComplainantUserId,
            request.TargetEntityType,
            request.TargetEntityId,
            request.ComplaintCategory,
            request.Description);

        dbContext.Complaints.Add(complaint);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(complaint);
    }

    public async Task<ComplaintResponse?> AssignAsync(Guid complaintId, Guid assignedTo, CancellationToken cancellationToken = default)
    {
        var complaint = await dbContext.Complaints.FirstOrDefaultAsync(c => c.ComplaintId == complaintId, cancellationToken);
        if (complaint is null)
        {
            return null;
        }

        var assigneeExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == assignedTo, cancellationToken);
        if (!assigneeExists)
        {
            throw new InvalidOperationException("Assignee user not found.");
        }

        complaint.Assign(assignedTo);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(complaint);
    }

    public async Task<ComplaintResponse?> ResolveAsync(Guid complaintId, ResolveComplaintRequest request, CancellationToken cancellationToken = default)
    {
        var complaint = await dbContext.Complaints.FirstOrDefaultAsync(c => c.ComplaintId == complaintId, cancellationToken);
        if (complaint is null)
        {
            return null;
        }

        complaint.Resolve(request.ResolutionNotes);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(complaint);
    }

    public async Task<ComplaintResponse?> RejectAsync(Guid complaintId, ResolveComplaintRequest request, CancellationToken cancellationToken = default)
    {
        var complaint = await dbContext.Complaints.FirstOrDefaultAsync(c => c.ComplaintId == complaintId, cancellationToken);
        if (complaint is null)
        {
            return null;
        }

        complaint.Reject(request.ResolutionNotes);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(complaint);
    }

    public async Task<ComplaintResponse?> EscalateAsync(Guid complaintId, CancellationToken cancellationToken = default)
    {
        var complaint = await dbContext.Complaints.FirstOrDefaultAsync(c => c.ComplaintId == complaintId, cancellationToken);
        if (complaint is null)
        {
            return null;
        }

        complaint.Escalate();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(complaint);
    }

    private static ComplaintResponse ToResponse(Complaint complaint) => new(
        complaint.ComplaintId,
        complaint.ComplainantUserId,
        complaint.TargetEntityType,
        complaint.TargetEntityId,
        complaint.ComplaintCategory,
        complaint.Description,
        complaint.Status.ToString(),
        complaint.AssignedTo,
        complaint.ResolutionNotes,
        complaint.CreatedAt,
        complaint.ResolvedAt);
}
