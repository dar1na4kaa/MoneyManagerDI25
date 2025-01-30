using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManagerX.Service
{
    public class TransactionService
    {
        public void AddTransaction(int selectedAccount, int selectedCategory, DateTime selectedDate, decimal amount, string desc, string type)
        {
            var transaction = new Transaction
            {
                AccountId = selectedAccount,
                CategoryId = selectedCategory,
                Date = selectedDate,
                Description = desc,
                Amount = amount,
                Type = type
            };
            using (var _dbcontext = new AccountingModel())
            {
                _dbcontext.Transactions.Add(transaction);
                _dbcontext.SaveChanges();
            }
        }
        public List<Transaction> GetIncomeForAccount(int accountId)
        {
            using (var _dbcontext = new AccountingModel())
            {
                var incomeTransaction = _dbcontext.Transactions
                                        .Where(c => c.AccountId == accountId).Where(c => c.Type.Equals("Доход"))
                                        .ToList();

                return incomeTransaction;
            }
        }
        public List<Transaction> GetSpendingForAccount(int accountId)
        {
            using (var _dbcontext = new AccountingModel())
            {
                var spendingTransaction = _dbcontext.Transactions
                                        .Where(c => c.AccountId == accountId).Where(c => c.Type.Equals("Расход"))
                                        .ToList();

                return spendingTransaction;
            }
        }
        public List<Transaction> GetIncomeForUser(int accountId, DateTime startDate, DateTime endDate, List<string> selectedCategories = null)
        {
            using (var _db = new AccountingModel())
            {
                var incomes = _db.Transactions
                                 .Where(t => t.AccountId == accountId &&
                                             t.Date >= startDate &&
                                             t.Date <= endDate &&
                                             t.Amount > 0)
                                 .ToList();

                if (selectedCategories != null && selectedCategories.Any())
                {
                    incomes = incomes.Where(t => selectedCategories.Contains(t.Category.Name)).ToList();
                }

                return incomes.ToList();
            }
        }

        public List<Transaction> GetSpendingForUser(int accId, DateTime startDate, DateTime endDate)
        {
            using (var db = new AccountingModel())
            {
                return db.Transactions
                         .Where(t => t.AccountId == accId && t.Date >= startDate && t.Date <= endDate && t.Amount > 0 && t.Type.Equals("Расход"))
                         .Include(t => t.Category)
                         .ToList();
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
