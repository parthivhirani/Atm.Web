using ATM.Web.Data;
using ATM.Web.Models;
using Humanizer;

namespace ATM.Web.LogRepository
{
    public class LogTransaction
    {
        private readonly BankDbContext _context;

        public LogTransaction(BankDbContext context)
        {
            _context  = context;
        }

        public void LogInfo(string from, string to, int amount, string type)
        {
            var log = new LogDetail()
            {
                FromAccount = from,
                ToAccount = to,
                AmountTransferred = amount,
                DateOfTransaction = DateTime.Now,
                TransactionType = type
            };
            _context.LogDetails.Add(log);
            _context.SaveChanges();
        }

        public void LogInfo(string from, int amount, string type)
        {
            var log = new LogDetail()
            {
                FromAccount = from,
                AmountTransferred = amount,
                DateOfTransaction = DateTime.Now,
                TransactionType = type
            };
            _context.LogDetails.Add(log);
            _context.SaveChanges();
        }
    }
}
