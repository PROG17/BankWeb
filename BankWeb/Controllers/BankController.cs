using BankWeb.Configs;
using BankWeb.Models;
using BankWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankWeb.Controllers
{
    public class BankController : Controller
    {
        private readonly IBankService _service;

        public BankController(IBankService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(Transaction transfer)
        {
            if (ModelState.IsValid)
            {
                var response = _service.Deposit(transfer.AccountNumber, transfer.Amount);

                if (response == BankResponse.NoAccount)
                    ModelState.AddModelError(nameof(Transaction.AccountNumber), "The account does not exist");

                if (ModelState.IsValid)
                    return View("TransactionSuccess", _service.GetAccountBalance(transfer.AccountNumber));
            }

            return View("Index", transfer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Withdraw(Transaction transfer)
        {
            if (ModelState.IsValid)
            {
                var response = _service.Withdraw(transfer.AccountNumber, transfer.Amount);

                if (response == BankResponse.NoAccount)
                    ModelState.AddModelError(nameof(Transaction.AccountNumber), "The account does not exist");

                if (response == BankResponse.NoFunds)
                    ModelState.AddModelError(nameof(Transaction.Amount), "Not enough funds on account");

                if (ModelState.IsValid)
                    return View("TransactionSuccess", _service.GetAccountBalance(transfer.AccountNumber));
            }
            return View("Index", transfer);
        }
    }
}