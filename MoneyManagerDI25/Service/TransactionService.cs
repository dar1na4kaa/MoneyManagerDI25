using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagerX.Service
{
    public class TransactionService
    {
        public List<Transaction> GetIncomeForUser(int userId)
        {
            using (var _dbcontext = new AccountingModel())
            {
                var incomeTransaction = _dbcontext.Transactions
                                        .Where(c => c.AccountId == 1).Where(c => c.Type.Equals("Доход"))
                                        .ToList();

                return incomeTransaction;
            }
        }
        public List<Transaction> GetSpendingForUser(int userId)
        {
            using (var _dbcontext = new AccountingModel())
            {
                var spendingTransaction = _dbcontext.Transactions
                                        .Where(c => c.AccountId == 1).Where(c => c.Type.Equals("Расход"))
                                        .ToList();

                return spendingTransaction;
            }
        }

        public void RecalculateBalance(int accountId,string action,decimal amount)
        {
            if (action.Equals("Доход")){
                PlusBalance(accountId,amount);
            }
            else
            {
                MinusBalance(accountId,amount);
            }
        }
        private void PlusBalance(int accountId,decimal count)
        {
            using (var _dbcontext = new AccountingModel())
            {
                Account acc = _dbcontext.Accounts
                                        .First(p => p.Id == accountId);
                acc.Balance += count;
                _dbcontext.SaveChanges();
            }
        }
        private void MinusBalance(int accountId, decimal count)
        {
            using (var _dbcontext = new AccountingModel())
            {
                Account acc = _dbcontext.Accounts
                                        .First(p => p.Id == accountId);
                acc.Balance -= count;
                _dbcontext.SaveChanges();
            }
        }
    }
}
