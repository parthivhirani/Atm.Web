using ATM.Web.Data;
using ATM.Web.Models;
using ATM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ATM.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly BankDbContext _context;

        public UserController(BankDbContext context)
        {
            _context = context;
        }
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var newUserBankDetail = new BankDetail()
                {
                    AccountNo = Guid.NewGuid().ToString(),
                    ATMCardNo = new Random().NextInt64(1000000000000000, 9999999999999999),
                    PIN = new Random().Next(1000, 9999),
                    Amount = 3000,
                };
                _context.BankDetails.Add(newUserBankDetail);
                _context.SaveChanges();


            }
            return View();
        }
    }
}
