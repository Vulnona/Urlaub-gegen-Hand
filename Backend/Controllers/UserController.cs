using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Users;
using UGHApi.Models;
using UGHApi.Services.UserProvider;
using UGHApi.DATA;

namespace UGHApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserProvider _userProvider;
    private readonly IMediator _mediator;

    public UserController(
        IMediator mediator,
        ILogger<UserController> logger,
        IUserProvider userProvider
    )
    {
        _mediator = mediator;
        _logger = logger;
        _userProvider = userProvider;
    }

    #region users-info


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

    // This function is to be used by the admin
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            var userId = _userProvider.UserId;
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
