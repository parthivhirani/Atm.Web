using ATM.Web.Data;
using ATM.Web.LogRepository;
using ATM.Web.Models;
using ATM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ATM.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly BankDbContext _context;
        private readonly LogTransaction _logger;

        public HomeController(BankDbContext context, LogTransaction logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(long cardNo)
        {
            if(cardNo == 0)
            {
                ViewBag.Error = "Enter valid ATM card number";
                return View(cardNo);
            }
            var account = _context.BankDetails.Where(a => a.ATMCardNo == cardNo).SingleOrDefault();
            if (account != null)
            {
                Response.Cookies.Append("acc_no", account.AccountNo);
                return RedirectToAction("AvailableServices");
            }
            ViewBag.Error = "User not found";
            return View();
        }


        public IActionResult AvailableServices()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CheckBalance()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckBalance(int pin)
        {
            var accNo = Request.Cookies["acc_no"];
            var account = _context.BankDetails.Where(a => a.AccountNo == accNo && a.PIN == pin).SingleOrDefault();
            if(account != null)
            {
                ViewBag.Error = null;
                ViewBag.Balance = "Your account balance is: ₹ " + account.Amount;
            }
            else
            {
                ViewBag.Balance = null;
                ViewBag.Error = "PIN is incorrect";
            }
            return View();
        }

        [HttpGet]
        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Withdraw(int amount, int pin)
        {
            if (amount >= 100)
            {
                var accNo = Request.Cookies["acc_no"];
                if (accNo != null)
                {
                    var account = _context.BankDetails.Where(a => a.AccountNo == accNo && a.PIN == pin).SingleOrDefault();
                    if(account == null)
                    {
                        ViewBag.Error = "PIN doesn't matched";
                        return View();
                    }
                    if(amount < account.Amount)
                    {
                        account.Amount -= amount;
                        _context.BankDetails.Update(account);
                        _context.SaveChanges();
                        _logger.LogInfo(accNo, amount, "WITHDRAW");
                        return View("WithdrawSuccess", account);
                    }
                    else
                    {
                        ViewBag.Error = "You are exceeding total amount";
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Error = "You have to withdraw minimum 100 rupees";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Credit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Credit(int amount, int pin)
        {
            if (amount >= 100)
            {
                var accNo = Request.Cookies["acc_no"];
                if (accNo != null)
                {
                    var account = _context.BankDetails.Where(a => a.AccountNo == accNo && a.PIN == pin).SingleOrDefault();
                    if(account == null)
                    {
                        ViewBag.Error = "PIN doesn't matched";
                        return View();
                    }
                    if(amount <= 20000)
                    {
                        account.Amount += amount;
                        _context.BankDetails.Update(account);
                        _context.SaveChanges();
                        _logger.LogInfo(accNo, amount, "CREDIT");
                        return View("CreditSuccess", account);
                    }
                    else
                    {
                        ViewBag.Error = "You can't credit more than 20,000 rupees using ATM";
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Error = "You have to credit minimum 100 rupees";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ChangePIN()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePIN(ChangePINViewModel changePINViewModel)
        {
            if (ModelState.IsValid)
            {
                if (changePINViewModel.OldPIN != changePINViewModel.NewPIN)
                {
                    var accNo = Request.Cookies["acc_no"];
                    var account = _context.BankDetails.Where(a => a.PIN == changePINViewModel.OldPIN && a.AccountNo == accNo).SingleOrDefault();
                    if (account != null)
                    {
                        account.PIN = changePINViewModel.NewPIN;
                        _context.BankDetails.Update(account);
                        _context.SaveChanges();
                        Response.Cookies.Delete("acc_no");
                        return View("ChangePINSuccess");
                    }
                    else
                    {
                        ViewBag.Error = "Please enter valid current PIN";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "New PIN and Old PIN can't be same";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult TransferAmount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TransferAmount(TransferViewModel transferViewModel)
        {
            if (ModelState.IsValid)
            {
                var accNo = Request.Cookies["acc_no"];
                var account = _context.BankDetails.Where(a => a.AccountNo == accNo && a.PIN == transferViewModel.PIN).SingleOrDefault();
                var recepientAccount = _context.BankDetails.Where(ra => ra.AccountNo == transferViewModel.AccountNo).SingleOrDefault();
            
                if (account == null || recepientAccount == null)
                {
                    ViewBag.Error = account == null ? "Your PIN doesn't matched" : "Invalid Account number";
                    return View(transferViewModel);
                }
                else
                {
                    if (account.AccountNo == recepientAccount.AccountNo)
                    {
                        ViewBag.Error = "You can't transfer amount to yourself!";
                        return View();
                    }
                    else if (transferViewModel.Amount >= account.Amount)
                    {
                        ViewBag.Error = "You are exceeding your total amount";
                        return View(transferViewModel);
                    }
                    else
                    {
                        var sumOfAmountBefore = account.Amount + recepientAccount.Amount;

                        account.Amount -= transferViewModel.Amount;
                        _context.BankDetails.Update(account);
                        var sender = _context.SaveChanges();

                        recepientAccount.Amount += transferViewModel.Amount;
                        _context.BankDetails.Update(recepientAccount);
                        var receiver = _context.SaveChanges();

                        var sumOfAmountAfter = account.Amount + recepientAccount.Amount;

                        if (sender == 1 && receiver == 1 && sumOfAmountBefore == sumOfAmountAfter)
                        {
                            _logger.LogInfo(account.AccountNo, recepientAccount.AccountNo, transferViewModel.Amount, "BANK-TRANSFER");
                            return View("TransferSuccess", transferViewModel);
                        }
                        else
                        {
                            return RedirectToAction("Error");
                        }
                    }
                }
            }
            else
            {
                ViewBag.Error = "Please enter valid bank details";
                return View(transferViewModel);
            }
        }


        public IActionResult Logout()
        {
            Response.Cookies.Delete("acc_no");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}