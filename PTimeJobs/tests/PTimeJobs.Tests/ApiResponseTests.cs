using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Tests;

public sealed class ApiResponseTests
{
    [Fact]
    public void Success_ShouldCreateStandardSuccessResponse()
    {
        var response = ApiResponse<string>.Success("ok");

        Assert.Equal("success", response.Status);
        Assert.Equal("Operation completed successfully.", response.Message);
        Assert.Equal("ok", response.Data);
        Assert.Null(response.Errors);
    }

    [Fact]
    public void Failure_ShouldCreateStandardErrorResponse()
    {
        var response = ApiResponse<string>.Failure("Validation failed.");

        Assert.Equal("error", response.Status);
        Assert.Equal("Validation failed.", response.Message);
        Assert.Null(response.Data);
    }
}
