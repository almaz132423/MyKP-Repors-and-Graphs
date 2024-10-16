using MyKP_работа_с_БД.DataBase;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace MyKP_работа_с_БД.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        private int idCategory = 0;
        private readonly Products current = null;
        public AddProduct(Products products)
        {
            InitializeComponent();

            System.Collections.Generic.List<Categories> categories = new DB_Operation().ExecuteQuery<Categories>("SELECT Name FROM Categories;");
            CategoryProd.Items.Clear();
            CategoryProd.ItemsSource = categories.Select(w => w.Name).ToList();

            System.Collections.Generic.List<Products> res = new DB_Operation().ExecuteQuery<Products>(
                @"SELECT p.*, 
                c.ID_Category as Categories_ID_Category, 
                c.Name as Categories_Name
                FROM Products p
                INNER JOIN Categories c ON p.ID_Category = c.ID_Category;");

            if (products != null)
            {
                current = res
                    .Where(w => w.ID_Product == products.ID_Product)
                    .ToList().LastOrDefault();
            }

            DataContext = current;
        }

        private void cbCategory_DropDownClosed(object sender, EventArgs e)
        {
            if (CategoryProd.SelectedIndex != -1)
            {
                System.Collections.Generic.List<Categories> categories = new DB_Operation().ExecuteQuery<Categories>("SELECT * FROM Categories;");
                idCategory = categories.Where(w => w.Name == CategoryProd.Text).LastOrDefault().ID_Category;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CategoryProd.SelectedIndex != -1)
                {
                    if (current != null)
                    {
                        string sql = "UPDATE Products SET " +
                            "Name = @Name, " +
                            "Description = @Description, " +
                            "Image = @Image, " +
                            "ID_Category = @ID_Category, " +
                            "Price = @Price " +
                            "WHERE ID_Product = @ProductID;";
                        SQLiteParameter[] parameters = [
                            new("@ProductID", current.ID_Product),
                            new("@Name", NameProd.Text),
                            new("@Description", DescriptoinPord.Text),
                            new("@Image", ImageProd.Text),
                            new("@ID_Category", idCategory),
                            new("@Price", PriceProd.Text)
                        ];

                        new DB_Operation().Query(sql, parameters);
                        _ = MessageBox.Show("Данные успешно изменены.", "Сохранено!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (current == null)
                    {
                        string sql = "INSERT INTO Products" +
                            "(Name, Description, Image, ID_Category, Price)" +
                            "VALUES (@Name, @Description, @Image, @ID_Category, @Price);";
                        SQLiteParameter[] parameters = [
                            new("@Name", NameProd.Text),
                            new("@Description", DescriptoinPord.Text),
                            new("@Image", ImageProd.Text),
                            new("@ID_Category", idCategory),
                            new("@Price", PriceProd.Text)
                        ];

                        new DB_Operation().Query(sql, parameters);

                        _ = MessageBox.Show("Данные успешно добавлены.", "Сохранено!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    NavigationService.GoBack();
                }
                else
                {
                    _ = MessageBox.Show("Выберите категорию товара.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Ошибка: " + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PriceProd_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new(@"^[0-9]*(?:[.,][0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (CategoryProd.SelectedIndex != -1)
            {
                CategoryProd.SelectedIndex += -1;
            }
        }
    }
}
