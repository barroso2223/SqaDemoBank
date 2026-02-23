using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqaDemoBank.Data;

namespace SqaDemoBank.Controllers
{
    public class AccountsController : Controller
    {
        private readonly BankingContext _context;

        public AccountsController(BankingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return View(accounts);
        }
    }
}
