using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class ContractsController(IContractsService contractsService) : ControllerBase
{
    [HttpGet("{contractId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid contractId, CancellationToken cancellationToken)
    {
        var contract = await contractsService.GetByIdAsync(contractId, cancellationToken);

        if (contract is null)
        {
            return NotFound(ApiResponse<ContractResponse>.Failure("Contract not found."));
        }

        return Ok(ApiResponse<ContractResponse>.Success(contract));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest request, CancellationToken cancellationToken)
    {
        var contract = await contractsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { contractId = contract.ContractId },
            ApiResponse<ContractResponse>.Success(contract, "Contract created."));
    }

    [HttpPatch("{contractId:guid}/activate")]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid contractId, CancellationToken cancellationToken)
    {
        var contract = await contractsService.ActivateAsync(contractId, cancellationToken);

        if (contract is null)
        {
            return NotFound(ApiResponse<ContractResponse>.Failure("Contract not found."));
        }

        return Ok(ApiResponse<ContractResponse>.Success(contract, "Contract activated."));
    }

    [HttpPatch("{contractId:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid contractId, [FromBody] EndContractRequest request, CancellationToken cancellationToken)
    {
        var contract = await contractsService.CompleteAsync(contractId, request, cancellationToken);

        if (contract is null)
        {
            return NotFound(ApiResponse<ContractResponse>.Failure("Contract not found."));
        }

        return Ok(ApiResponse<ContractResponse>.Success(contract, "Contract completed."));
    }

    [HttpPatch("{contractId:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid contractId, CancellationToken cancellationToken)
    {
        var contract = await contractsService.CancelAsync(contractId, cancellationToken);

        if (contract is null)
        {
            return NotFound(ApiResponse<ContractResponse>.Failure("Contract not found."));
        }

        return Ok(ApiResponse<ContractResponse>.Success(contract, "Contract cancelled."));
    }

    [HttpPatch("{contractId:guid}/terminate")]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ContractResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Terminate(Guid contractId, [FromBody] EndContractRequest request, CancellationToken cancellationToken)
    {
        var contract = await contractsService.TerminateAsync(contractId, request, cancellationToken);

        if (contract is null)
        {
            return NotFound(ApiResponse<ContractResponse>.Failure("Contract not found."));
        }

        return Ok(ApiResponse<ContractResponse>.Success(contract, "Contract terminated."));
    }
}
