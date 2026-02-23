using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqaDemoBank.Data;
using SqaDemoBank.Models;

namespace SqaDemoBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsApiController : ControllerBase
    {
        private readonly BankingContext _context;

        public AccountsApiController(BankingContext context)
        {
            _context = context;
        }

        // GET: /api/accountsapi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: /api/accountsapi/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            return account;
        }
    }
}
