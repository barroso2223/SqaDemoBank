using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqaDemoBank.Data;
using SqaDemoBank.Models;

namespace SqaDemoBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransfersApiController : ControllerBase
    {
        private readonly BankingContext _context;

        public TransfersApiController(BankingContext context)
        {
            _context = context;
        }

       
        // POST: /api/transfersapi
        [HttpPost]
        public async Task<IActionResult> CreateTransfer([FromBody] TransferRequestDto request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest(new { message = "Amount must be greater than zero." });
            }

            if (request.FromAccountId == request.ToAccountId)
            {
                return BadRequest(new { message = "Source and destination accounts must be different." });
            }

            var from = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == request.FromAccountId);
            var to = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == request.ToAccountId);

            if (from == null || to == null)
            {
                return NotFound(new { message = "One or both accounts not found." });
            }

            if (from.Balance < request.Amount)
            {
                return BadRequest(new { message = "Insufficient funds." });
            }

            from.Balance -= request.Amount;
            to.Balance += request.Amount;

            var transfer = new Transfer
            {
                FromAccountId = from.Id,
                ToAccountId = to.Id,
                Amount = request.Amount
            };

            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTransfer),
                new { id = transfer.Id },
                new
                {
                    transfer.Id,
                    transfer.Amount,
                    transfer.Timestamp,
                    Status = "success"
                });
        }

        // GET: /api/transfersapi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transfer>> GetTransfer(int id)
        {
            var transfer = await _context.Transfers.FindAsync(id);
            if (transfer == null)
                return NotFound();

            return transfer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transfer>>> GetTransfers()
        {
            var transfers = await _context.Transfers.ToListAsync();
            return transfers;
        }

    }
}
