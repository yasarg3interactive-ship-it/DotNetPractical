namespace PTimeJobs.Application.Common.Models;

public sealed record ApiResponse<T>(
    string Status,
    string Message,
    T? Data,
    IReadOnlyDictionary<string, string[]>? Errors = null)
{
    public static ApiResponse<T> Success(T? data, string message = "Operation completed successfully.")
    {
        return new ApiResponse<T>("success", message, data);
    }

    public static ApiResponse<T> Failure(string message, IReadOnlyDictionary<string, string[]>? errors = null)
    {
        return new ApiResponse<T>("error", message, default, errors);
    }
}
