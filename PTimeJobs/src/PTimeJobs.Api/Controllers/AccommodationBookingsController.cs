using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class AccommodationBookingsController(IAccommodationBookingsService accommodationBookingsService) : ControllerBase
{
    [HttpGet("{bookingId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await accommodationBookingsService.GetByIdAsync(bookingId, cancellationToken);

        if (booking is null)
        {
            return NotFound(ApiResponse<AccommodationBookingResponse>.Failure("Booking not found."));
        }

        return Ok(ApiResponse<AccommodationBookingResponse>.Success(booking));
    }

    [HttpGet("by-worker/{workerProfileId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<AccommodationBookingResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByWorker(Guid workerProfileId, CancellationToken cancellationToken)
    {
        var bookings = await accommodationBookingsService.GetByWorkerAsync(workerProfileId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<AccommodationBookingResponse>>.Success(bookings));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccommodationBookingRequest request, CancellationToken cancellationToken)
    {
        var booking = await accommodationBookingsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { bookingId = booking.BookingId },
            ApiResponse<AccommodationBookingResponse>.Success(booking, "Booking requested."));
    }

    [HttpPatch("{bookingId:guid}/confirm")]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await accommodationBookingsService.ConfirmAsync(bookingId, cancellationToken);

        if (booking is null)
        {
            return NotFound(ApiResponse<AccommodationBookingResponse>.Failure("Booking not found."));
        }

        return Ok(ApiResponse<AccommodationBookingResponse>.Success(booking, "Booking confirmed."));
    }

    [HttpPatch("{bookingId:guid}/check-in")]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckIn(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await accommodationBookingsService.CheckInAsync(bookingId, cancellationToken);

        if (booking is null)
        {
            return NotFound(ApiResponse<AccommodationBookingResponse>.Failure("Booking not found."));
        }

        return Ok(ApiResponse<AccommodationBookingResponse>.Success(booking, "Checked in."));
    }

    [HttpPatch("{bookingId:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid bookingId, [FromBody] CompleteBookingRequest request, CancellationToken cancellationToken)
    {
        var booking = await accommodationBookingsService.CompleteAsync(bookingId, request, cancellationToken);

        if (booking is null)
        {
            return NotFound(ApiResponse<AccommodationBookingResponse>.Failure("Booking not found."));
        }

        return Ok(ApiResponse<AccommodationBookingResponse>.Success(booking, "Booking completed."));
    }

    [HttpPatch("{bookingId:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await accommodationBookingsService.CancelAsync(bookingId, cancellationToken);

        if (booking is null)
        {
            return NotFound(ApiResponse<AccommodationBookingResponse>.Failure("Booking not found."));
        }

        return Ok(ApiResponse<AccommodationBookingResponse>.Success(booking, "Booking cancelled."));
    }

    [HttpPatch("{bookingId:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AccommodationBookingResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await accommodationBookingsService.RejectAsync(bookingId, cancellationToken);

        if (booking is null)
        {
            return NotFound(ApiResponse<AccommodationBookingResponse>.Failure("Booking not found."));
        }

        return Ok(ApiResponse<AccommodationBookingResponse>.Success(booking, "Booking rejected."));
    }
}
