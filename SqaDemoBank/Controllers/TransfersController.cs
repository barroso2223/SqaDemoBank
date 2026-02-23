using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqaDemoBank.Data;
using SqaDemoBank.Models;

namespace SqaDemoBank.Controllers
{
    public class TransfersController : Controller
    {
        private readonly BankingContext _context;

        public TransfersController(BankingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Accounts = await _context.Accounts.ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var transfers = await _context.Transfers
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .ToListAsync();

            return View(transfers);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransferRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Accounts = await _context.Accounts.ToListAsync();
                return View(request);
            }

            // You can reuse the validation logic from the API here,
            // or call the API itself if you want to be fancy.
            var from = await _context.Accounts.FindAsync(request.FromAccountId);
            var to = await _context.Accounts.FindAsync(request.ToAccountId);

            if (from == null || to == null)
            {
                ModelState.AddModelError("", "Invalid account selection.");
            }
            else if (from.Balance < request.Amount)
            {
                ModelState.AddModelError("", "Insufficient funds.");
            }
            else
            {
                from.Balance -= request.Amount;
                to.Balance += request.Amount;

                _context.Transfers.Add(new Transfer
                {
                    FromAccountId = from.Id,
                    ToAccountId = to.Id,
                    Amount = request.Amount
                });

                await _context.SaveChangesAsync();
                TempData["Message"] = "Transfer completed successfully.";
                return RedirectToAction("Index", "Accounts");
            }

            ViewBag.Accounts = await _context.Accounts.ToListAsync();
            return View(request);
        }
    }
}
