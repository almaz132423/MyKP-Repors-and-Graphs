using MyKP_работа_с_БД.DataBase;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MyKP_работа_с_БД.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewProduct.xaml
    /// </summary>
    public partial class ViewProduct : Page
    {
        private IEnumerable<Products> result = [];

        public ViewProduct()
        {
            InitializeComponent();
            View();
            Load();

            List<Categories> categories = new DB_Operation().ExecuteQuery<Categories>("SELECT Name FROM Categories;");

            cbFilter.Items.Clear();

            _ = cbFilter.Items.Add("Без фильтров");
            foreach (Categories category in categories)
            {
                _ = cbFilter.Items.Add(category.Name);
            }

            //cbFilter.ItemsSource = categories.Select(w => w.Name).ToList();
        }

        private void View()
        {
            result = new DB_Operation().ExecuteQuery<Products>(
                @"SELECT p.*, 
                c.ID_Category as Categories_ID_Category, 
                c.Name as Categories_Name
                FROM Products p
                INNER JOIN Categories c ON p.ID_Category = c.ID_Category;");
        }

        private void Load()
        {
            lbProduct.Items.Clear();
            foreach (Products prod in result)
            {
                _ = lbProduct.Items.Add(prod);
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbSearch.Text != "")
            {
                result = result.Where(w => w.Name.ToLower().Contains(tbSearch.Text.ToLower()));
            }

            Load();
        }

        private void cbFilter_DropDownClosed(object sender, EventArgs e)
        {
            if (cbFilter.SelectedItem != null)
            {
                if (cbFilter.Text == "Без фильтров")
                {
                    result = new DB_Operation().ExecuteQuery<Products>(
                        @"SELECT p.*, 
                        c.ID_Category as Categories_ID_Category, 
                        c.Name as Categories_Name
                        FROM Products p
                        INNER JOIN Categories c ON p.ID_Category = c.ID_Category;");
                    Load();
                }
                else
                {
                    result = result.Where(w => w.Categories.Name.ToLower().Contains(cbFilter.Text.ToLower()));
                    Load();
                }
            }

        }

        private void btn_DeleteClick(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is Products delete)
            {
                if (MessageBox.Show($"Вы точно хотите удалить выбранный элемент?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        string sql = "DELETE FROM Products WHERE ID_Product = @Id";
                        SQLiteParameter[] parameters = [
                            new("@Id", delete.ID_Product)
                        ];

                        new DB_Operation().Query(sql, parameters);

                        _ = MessageBox.Show("Записи удалены.", "Внимение!", MessageBoxButton.OK, MessageBoxImage.Information);
                        View();
                        Load();
                    }
                    catch
                    {
                        _ = MessageBox.Show("Записи не удалены.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

        private void btn_UpdateClick(object sender, RoutedEventArgs e)
        {
            _ = NavigationService.Navigate(
                new Pages.AddProduct(
                (sender as Button).DataContext as Products));
        }

        private void btn_ReportClick(object sender, RoutedEventArgs e)
        {
            _ = NavigationService.Navigate(new ViewReport());
        }

        private void btn_AddClick(object sender, RoutedEventArgs e)
        {
            _ = NavigationService.Navigate(new Pages.AddProduct(null));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            View();
            Load();
        }
    }
}
