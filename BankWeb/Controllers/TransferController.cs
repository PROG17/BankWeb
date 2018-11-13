using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankWeb.Configs;
using BankWeb.Models;
using BankWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankWeb.Controllers
{
    public class TransferController : Controller
    {
        private readonly IBankService _service;

        public TransferController(IBankService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {          

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transfer(Transaction transfer)
        {
            if (ModelState.IsValid)
            {
                var response = _service.Transfer(transfer.AccountNumber, transfer.ToAccountNumber, transfer.Amount);

                if (response == BankResponse.NoAccount)
                    ModelState.AddModelError(nameof(Transaction.AccountNumber), "The account does not exist");

                if (response == BankResponse.NoFunds)
                    ModelState.AddModelError(nameof(Transaction.Amount), "Not enough funds on account");

                if (ModelState.IsValid)
                    return View("TransferSuccess", _service.GetTransferDetails(transfer.AccountNumber, transfer.ToAccountNumber));
            }
            return View("Index", transfer);
        }
    }
}