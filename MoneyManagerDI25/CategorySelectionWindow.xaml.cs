using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MoneyManagerX.Service;

namespace MoneyManagerX
{
    public partial class CategorySelectionWindow : Window
    {
        private readonly User _user;
        private List<Category> _selectedCategories;
        private readonly CategoryService _categoryService;

        public CategorySelectionWindow(User user)
        {
            InitializeComponent();
            _user = user;
            _selectedCategories = new List<Category>(); 
            _categoryService = new CategoryService();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var allCategories = _categoryService.GetUserCategories(_user.Id);
            CategoryListBox.ItemsSource = allCategories;
        }

        private void SaveSelectedCategories(object sender, RoutedEventArgs e)
        {
            _selectedCategories = CategoryListBox.SelectedItems.Cast<Category>().ToList();
            this.Close();
        }

        public List<Category> GetSelectedCategories()
        {
            return _selectedCategories;
        }
    }
}
