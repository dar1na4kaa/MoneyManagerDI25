using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagerX.Service
{
    public class AccountService
    {
        public void AddAccount(string accountName,decimal balance,int userId)
        {
            var newAccount = new Account
            {
                Name = accountName,
                Balance = balance,
                UserId = userId
            };

            using (var dbContext = new AccountingModel())
            {
                dbContext.Accounts.Add(newAccount);
                dbContext.SaveChanges();
            }
        }
    }
}
