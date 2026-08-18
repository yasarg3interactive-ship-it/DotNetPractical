namespace PTimeJobs.Domain.Jobs;

public sealed class JobCategory
{
    private JobCategory()
    {
    }

    public Guid JobCategoryId { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public string CategoryName { get; private set; } = string.Empty;
    public string CategorySlug { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public static JobCategory Create(string categoryName, string categorySlug, Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            throw new InvalidOperationException("Category name is required.");
        }

        if (string.IsNullOrWhiteSpace(categorySlug))
        {
            throw new InvalidOperationException("Category slug is required.");
        }

        return new JobCategory
        {
            JobCategoryId = Guid.NewGuid(),
            ParentCategoryId = parentCategoryId,
            CategoryName = categoryName,
            CategorySlug = categorySlug,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
