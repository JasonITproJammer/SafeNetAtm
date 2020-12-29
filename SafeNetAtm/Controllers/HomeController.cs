using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SafeNetAtm.Models;
using SafeNetAtm.Domain;

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
            var model = new AtmViewModel();
            try
            {
                model.InventoryList = _atmRepository.Balance;
            }
            catch (Exception ex)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage = ex.Message;
            }
            return PartialView("_CurrentBalance",model);
        }

        public IActionResult Withdraw(AtmViewModel model)
        {
            try
            {
                _atmRepository.Withdraw(model.WithdrawalAmount);
                model.InventoryList = _atmRepository.Balance;
            }
            catch (Exception ex)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage = ex.Message;
            }
            return PartialView("_Withdraw", model);
        }

        public IActionResult DenominationBalance(AtmViewModel model)
        {
            try
            {
                model.InventoryList = new List<Inventory>();
                foreach (var d in model.Denomination)
                {
                    model.InventoryList.Append(_atmRepository.DenominationBalance(d));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage = ex.Message;
            }
            return PartialView("_DenominationBalance", model);
        }

        public IActionResult Restock(AtmViewModel model)
        {
            try
            {
                _atmRepository.Restock();
                model.InventoryList = _atmRepository.Balance;
            }
            catch (Exception ex)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage = ex.Message;
            }
            return PartialView("_Restock", model);
        }
    }
}
