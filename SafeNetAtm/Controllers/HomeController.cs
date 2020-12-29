using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [ValidateAntiForgeryToken()]
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
            return PartialView("_Balance",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public IActionResult DenominationBalance(AtmViewModel model)
        {
            try
            {
                List<Inventory> list = new List<Inventory>();
                foreach (var d in model.Denomination)
                {
                    list.Add(_atmRepository.DenominationBalance(d)); 
                }
                model.InventoryList = list;
            }
            catch (Exception ex)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage = ex.Message;
            }
            return PartialView("_Balance", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public IActionResult Withdraw(AtmViewModel model)
        {
            try
            {
                _atmRepository.Withdraw(model.WithdrawalAmount);
            }
            catch (Exception ex)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage += ex.Message;
            }

            try
            {
                model.InventoryList = _atmRepository.Balance;
            }
            catch (Exception ex)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage += ex.Message;
            }

            return PartialView("_Balance", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
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
            return PartialView("_Balance", model);
        }
    }
}
