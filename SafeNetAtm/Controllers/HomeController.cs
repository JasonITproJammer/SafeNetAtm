using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SafeNetAtm.Models;

namespace SafeNetAtm.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInventory _atmRepository;

        public HomeController(IInventory atmRepository)
        {
            _atmRepository = atmRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CurrentBalance()
        {
            var model = new AtmViewModel
            {
                InventoryList = _atmRepository.Balance
            };
            return PartialView("_CurrentBalance",model);
        }

        public IActionResult Withdraw(int amount)
        {
            AtmViewModel model = new AtmViewModel
            {
                WithdrawalAmount = amount
            };
            _atmRepository.Withdraw(model.WithdrawalAmount);
            model.InventoryList = _atmRepository.Balance;
            return PartialView("_Withdraw", model);
        }
    }
}
