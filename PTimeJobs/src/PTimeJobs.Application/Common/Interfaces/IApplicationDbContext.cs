using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Domain.Billing;
using PTimeJobs.Domain.Complaints;
using PTimeJobs.Domain.Employers;
using PTimeJobs.Domain.Food;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Locations;
using PTimeJobs.Domain.Messaging;
using PTimeJobs.Domain.Notifications;
using PTimeJobs.Domain.Reports;
using PTimeJobs.Domain.Reviews;
using PTimeJobs.Domain.Users;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    IQueryable<User> Users { get; }
    IQueryable<Role> Roles { get; }
    IQueryable<UserRole> UserRoles { get; }
    IQueryable<UserProfile> UserProfiles { get; }
    IQueryable<UserSession> UserSessions { get; }
    IQueryable<Verification> Verifications { get; }
    IQueryable<Country> Countries { get; }
    IQueryable<State> States { get; }
    IQueryable<City> Cities { get; }
    IQueryable<Area> Areas { get; }
    IQueryable<Location> Locations { get; }
    IQueryable<Skill> Skills { get; }
    IQueryable<EmployerProfile> EmployerProfiles { get; }
    IQueryable<WorkerProfile> WorkerProfiles { get; }
    IQueryable<WorkerSkill> WorkerSkills { get; }
    IQueryable<WorkerExperience> WorkerExperiences { get; }
    IQueryable<WorkerEducation> WorkerEducations { get; }
    IQueryable<WorkerDocument> WorkerDocuments { get; }
    IQueryable<WorkerAvailability> WorkerAvailabilities { get; }
    IQueryable<JobCategory> JobCategories { get; }
    IQueryable<Job> Jobs { get; }
    IQueryable<JobLocation> JobLocations { get; }
    IQueryable<JobSchedule> JobSchedules { get; }
    IQueryable<JobSkill> JobSkills { get; }
    IQueryable<JobApplication> JobApplications { get; }
    IQueryable<HiringStatusHistory> HiringStatusHistories { get; }
    IQueryable<MatchingScore> MatchingScores { get; }
    IQueryable<Conversation> Conversations { get; }
    IQueryable<ConversationParticipant> ConversationParticipants { get; }
    IQueryable<Message> Messages { get; }
    IQueryable<MessageAttachment> MessageAttachments { get; }
    IQueryable<Permission> Permissions { get; }
    IQueryable<RolePermission> RolePermissions { get; }
    IQueryable<Notification> Notifications { get; }
    IQueryable<Review> Reviews { get; }
    IQueryable<Complaint> Complaints { get; }
    IQueryable<Report> Reports { get; }
    IQueryable<Shortlist> Shortlists { get; }
    IQueryable<FoodProvider> FoodProviders { get; }
    IQueryable<FoodItem> FoodItems { get; }
    IQueryable<FoodPlan> FoodPlans { get; }
    IQueryable<FoodPlanItem> FoodPlanItems { get; }
    IQueryable<FoodSubscription> FoodSubscriptions { get; }
    IQueryable<DeliveryArea> DeliveryAreas { get; }
    IQueryable<AccommodationProvider> AccommodationProviders { get; }
    IQueryable<Property> Properties { get; }
    IQueryable<PropertyFacility> PropertyFacilities { get; }
    IQueryable<PropertyImage> PropertyImages { get; }
    IQueryable<Facility> Facilities { get; }
    IQueryable<RoomType> RoomTypes { get; }
    IQueryable<Room> Rooms { get; }
    IQueryable<RoomAvailability> RoomAvailabilities { get; }
    IQueryable<AccommodationBooking> AccommodationBookings { get; }
    IQueryable<Contract> Contracts { get; }
    IQueryable<BillingSubscription> BillingSubscriptions { get; }
    IQueryable<Invoice> Invoices { get; }
    IQueryable<Payment> Payments { get; }
    IQueryable<Transaction> Transactions { get; }
    IQueryable<AuditLog> AuditLogs { get; }
    IQueryable<AnalyticsEvent> AnalyticsEvents { get; }
    IQueryable<UserBehaviorEvent> UserBehaviorEvents { get; }
    IQueryable<SearchHistory> SearchHistories { get; }
    IQueryable<RecommendationHistory> RecommendationHistories { get; }
    IQueryable<UserPreference> UserPreferences { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
