using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManagerX.Service
{
    public class CategoryService
    {
        private readonly AccountingModel _db;

        public CategoryService()
        {
            _db = new AccountingModel();
        }
        public void AddCategory(int userId, string categoryName)
        {
            using (var db = new AccountingModel())
            {
                if (db.Categories.Any(c => c.UserId == userId && c.Name == categoryName))
                    throw new Exception("Такая категория уже существует!");

                var category = new Category
                {
                    UserId = userId,
                    Name = categoryName
                };

                db.Categories.Add(category);
                db.SaveChanges();
            }
        }
        public List<Category> GetUserCategories(int userId)
        {
            using (var _db = new AccountingModel())
            {
                return _db.Users
                         .Where(u => u.Id == userId)
                         .SelectMany(u => u.Categories)
                         .ToList();
            }
        }
    }
}
