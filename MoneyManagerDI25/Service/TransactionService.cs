using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public List<Transaction> GetTransactionsForAccount(int accountId, string transactionType)
        {
            using (var dbContext = new AccountingModel())
            {
                var transactions = dbContext.Transactions
                    .Where(c => c.AccountId == accountId && c.Type.Equals(transactionType))
                    .ToList();

                return transactions;
            }
        }
        public List<Transaction> GetTransactionsForUser(int accountId, DateTime startDate, DateTime endDate, string transactionType)
        {
            using (var dbContext = new AccountingModel())
            {
                var transactions = dbContext.Transactions
                    .Where(i => i.AccountId == accountId && i.Date >= startDate && i.Date <= endDate && i.Amount > 0 && i.Type.Equals(transactionType))
                    .Include(i => i.Category)
                    .ToList();

                var groupedTransactions = transactions
                    .GroupBy(t => t.Category.Name)
                    .Select(g => new
                    {
                        CategoryName = g.Key,
                        TotalAmount = g.Sum(t => t.Amount)
                    })
                    .ToList();

                var result = groupedTransactions.Select(g => new Transaction
                {
                    Category = new Category { Name = g.CategoryName },
                    Amount = g.TotalAmount,
                    Type = transactionType
                }).ToList();

                return result;
            }
        }
        public decimal GetAllTransactionBalance(int accountId, string transactionType)
        {
            decimal result = 0;
            using (var _dbcontext = new AccountingModel())
            {
                var transactions = _dbcontext.Transactions
                                    .Where(i => i.AccountId == accountId && i.Type.Equals(transactionType))
                                    .ToList();

                return result = transactions
                    .Sum(t => t.Amount);
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
