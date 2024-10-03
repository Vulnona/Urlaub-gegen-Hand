using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Users;
using UGHApi.Models;
using MediatR;

namespace UGHApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    #region users-info

    [Authorize]
    [HttpGet("get-user-by-id/{id}")]
    public async Task<IActionResult> GetUser([Required] Guid id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPut("upload-id")]
    public async Task<IActionResult> UploadID([FromBody] UploadIdRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var command = new UploadIdCommand(model.Id, model.Link_VS, model.Link_RS);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok("ID Uploaded Successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpDelete("delete-user/{userId}")]
    public async Task<IActionResult> DeleteUser([Required] Guid userId)
    {
        try
        {
            var command = new DeleteUserCommand(userId);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    #endregion
}
