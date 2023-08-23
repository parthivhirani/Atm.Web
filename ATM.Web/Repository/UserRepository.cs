using ATM.Web.Data;
using ATM.Web.Models;
using ATM.Web.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace ATM.Web.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BankDbContext _context;

        public UserRepository(BankDbContext context)
        {
            _context = context;
        }

        //public bool CreateUser(RegisterViewModel registerViewModel)
        //{
        //    var newUserBankDetail = new BankDetail()
        //    {
        //        AccountNo = Guid.NewGuid().ToString(),
        //        ATMCardNo = new Random().NextInt64(1000000000000000, 9999999999999999),
        //        PIN = new Random().Next(1000, 9999),
        //        Amount = 3000,
        //    };
        //    _context.BankDetails.Add(newUserBankDetail);
        //    var x = _context.SaveChanges();

        //    var newUser = new Customer()
        //    {
        //        FirstName = registerViewModel.FirstName,
        //        LastName = registerViewModel.LastName,
        //        ContactNo = registerViewModel.ContactNo,
        //        Email = registerViewModel.Email,
        //        BankDetails = newUserBankDetail,
        //        PasswordHash = registerViewModel.Password
        //    };
        //    _context.Customers.Add(newUser);
        //    var y = _context.SaveChanges();

        //    if (x == 1 && y == 1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public RegistrationSuccessViewModel CreateUser(RegisterViewModel registerViewModel)
        {
            var newUserBankDetail = new BankDetail()
            {
                AccountNo = Guid.NewGuid().ToString(),
                ATMCardNo = new Random().NextInt64(1000000000000000, 9999999999999999),
                PIN = new Random().Next(1000, 9999),
                Amount = 3000,
            };
            _context.BankDetails.Add(newUserBankDetail);
            var x = _context.SaveChanges();

            var newUser = new Customer()
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                ContactNo = registerViewModel.ContactNo,
                Email = registerViewModel.Email,
                BankDetails = newUserBankDetail,
                PasswordHash = getHash(registerViewModel.Password)
            };
            _context.Customers.Add(newUser);
            var y = _context.SaveChanges();

            if (x == 1 && y == 1)
            {
                return new RegistrationSuccessViewModel()
                {
                    FullName = String.Join(" ", registerViewModel.FirstName, registerViewModel.LastName),
                    AccountNo = newUserBankDetail.AccountNo,
                    ATMCardNo = newUserBankDetail.ATMCardNo,
                    PIN = newUserBankDetail.PIN
                };
            }
            else
            {
                return new RegistrationSuccessViewModel();
            }
        }

        private static string getHash(string password)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
