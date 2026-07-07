using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Interfaces;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    IQueryable<User> IApplicationDbContext.Users => Users;
    IQueryable<Role> IApplicationDbContext.Roles => Roles;
    IQueryable<UserRole> IApplicationDbContext.UserRoles => UserRoles;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
