using Microsoft.AspNetCore.Identity;
using PTimeJobs.Application.Common.Interfaces;

namespace PTimeJobs.Infrastructure.Services;

/// <summary>
/// Wraps ASP.NET Core Identity's PasswordHasher (PBKDF2). The generic TUser parameter of the
/// underlying hasher is never actually read by its implementation, so a plain marker object is
/// used instead of coupling this to the Domain's User entity.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private static readonly object Subject = new();

    private readonly Microsoft.AspNetCore.Identity.PasswordHasher<object> _inner = new();

    public string Hash(string password) => _inner.HashPassword(Subject, password);

    public bool Verify(string password, string passwordHash)
    {
        var result = _inner.VerifyHashedPassword(Subject, passwordHash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
