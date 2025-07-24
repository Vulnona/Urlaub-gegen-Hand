using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGHApi.Applications.Transactions;
using UGHApi.Services.UserProvider;

namespace UGHApi.Controllers
{
    [Route("api/transaction")]
    [Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(
            ILogger<TransactionController> logger,
            IMediator mediator,
            IUserProvider userProvider
        )
        {
            _mediator = mediator;
            _logger = logger;
            _userProvider = userProvider;
        }

        [HttpGet("get-user-transactions")]
        public async Task<IActionResult> GetUserTransactions(int pageNumber = 1, int pageSize = 10)
        {
            var userId = _userProvider.UserId;
            var result = await _mediator.Send(
                new GetUserTransactionQuery(userId, pageNumber, pageSize)
            );

            return Ok(result.Value);
        }

        [HttpPost("get-payment-intent")]
        public async Task<IActionResult> GetPendingPaymentIntent(int transactionId)
        {
            var userId = _userProvider.UserId;
            var query = new GetPendingPaymentIntentIdQuery
            {
                UserId = userId,
                TransactionId = transactionId,
            };

            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return Ok(result);
            }

            return Ok(result);
        }
    }
}
