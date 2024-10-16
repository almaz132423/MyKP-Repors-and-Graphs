using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using LiveCharts;
using LiveCharts.Wpf;
using MyKP_работа_с_БД.DataBase;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyKP_работа_с_БД.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewReport.xaml
    /// </summary>
    public partial class ViewReport : Page
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Categories { get; set; }
        public Func<double, string> Formatter { get; set; }


        private readonly IEnumerable<Products> result;

        public ViewReport()
        {
            InitializeComponent();

            result = new DB_Operation().ExecuteQuery<Products>(
                                            @"SELECT p.*, 
                                            c.ID_Category as Categories_ID_Category, 
                                            c.Name as Categories_Name
                                            FROM Products p
                                            INNER JOIN Categories c 
                                            ON p.ID_Category = c.ID_Category;");

            foreach (Products item in result)
            {
                _ = ProductDataGrid.Items.Add(item);
            }
        }

        public void GenerateWordReport(IEnumerable<Products> products)
        {
            string folderPath = @"C:\Reports";
            string fileName = $"{folderPath}\\ProductReport_{DateTime.Now:yyyyMMddHHmmss}.docx";

            // Создание папки, если она не существует
            if (!System.IO.Directory.Exists(folderPath))
            {
                _ = System.IO.Directory.CreateDirectory(folderPath);
            }

            // Создание Word документа
            using (WordprocessingDocument wordDocument
                = WordprocessingDocument.Create(fileName,
                DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new();

                // Добавляем заголовок отчёта
                Paragraph title = new(new Run(new Text("Самый лучший отчет!")))
                {
                    ParagraphProperties = new ParagraphProperties
                    {
                        Justification = new Justification { Val = JustificationValues.Center }
                    }
                };
                body.Append(title);

                // Создаем таблицу
                Table table = new();

                // Определяем свойства таблицы (границы, выравнивание и т.д.)
                TableProperties tblProps = new(
                    new TableBorders(
                        new TopBorder { Val = BorderValues.Single, Size = 6 },
                        new BottomBorder { Val = BorderValues.Single, Size = 6 },
                        new LeftBorder { Val = BorderValues.Single, Size = 6 },
                        new RightBorder { Val = BorderValues.Single, Size = 6 },
                        new InsideHorizontalBorder { Val = BorderValues.Single, Size = 6 },
                        new InsideVerticalBorder { Val = BorderValues.Single, Size = 6 }
                    )
                );
                _ = table.AppendChild(tblProps);

                // Создаем строку заголовков
                TableRow headerRow = new();

                headerRow.Append(
                    CreateCell("Номер", true),
                    CreateCell("Название", true),
                    CreateCell("Описание", true),
                    CreateCell("Категория", true),
                    CreateCell("Цена", true)
                );
                table.Append(headerRow);

                // Добавляем строки с данными
                foreach (Products product in products)
                {
                    TableRow row = new();
                    row.Append(
                        CreateCell(product.ID_Product.ToString()),
                        CreateCell(product.Name),
                        CreateCell(product.Description),
                        CreateCell(product.Categories.Name),
                        CreateCell($"{product.Price} руб.")
                    );
                    table.Append(row);
                }

                // Добавляем таблицу в тело документа
                body.Append(table);

                // Сохраняем документ
                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            // Открываем файл после создания
            _ = Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });

            _ = MessageBox.Show("Отчет в Word создан!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private TableCell CreateCell(string text, bool isHeader = false)
        {
            // Создаём ячейку таблицы
            TableCell cell = new();

            // Создаём параграф с текстом
            Paragraph paragraph = new(new Run(new Text(text)));

            if (isHeader)
            {
                // Устанавливаем жирный шрифт для заголовков
                paragraph.ParagraphProperties = new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                );
                paragraph.Descendants<Run>().FirstOrDefault().RunProperties = new RunProperties
                {
                    Bold = new Bold()
                };
            }

            // Добавляем параграф в ячейку
            cell.Append(paragraph);

            // Возвращаем готовую ячейку
            return cell;
        }

        public void GenerateExcelReport(IEnumerable<Products> products)
        {
            string folderPath = @"C:\Reports";
            string fileName = $"{folderPath}\\ProductReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            // Создаем папку, если она не существует
            if (!Directory.Exists(folderPath))
            {
                _ = Directory.CreateDirectory(folderPath);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Создаем Excel файл
            FileInfo newFile = new(fileName);
            using (ExcelPackage package = new(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");

                // Заголовок отчёта
                worksheet.Cells[1, 1].Value = "Самый лучший отчёт!";
                worksheet.Cells[1, 1, 1, 5].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Заголовки столбцов
                worksheet.Cells[3, 1].Value = "Номер";
                worksheet.Cells[3, 2].Value = "Название";
                worksheet.Cells[3, 3].Value = "Описание";
                worksheet.Cells[3, 4].Value = "Категория";
                worksheet.Cells[3, 5].Value = "Цена";

                // Данные продуктов
                int row = 4;
                foreach (Products product in products)
                {
                    worksheet.Cells[row, 1].Value = product.ID_Product;
                    worksheet.Cells[row, 2].Value = product.Name;
                    worksheet.Cells[row, 3].Value = product.Description;
                    worksheet.Cells[row, 4].Value = product.Categories.Name;
                    worksheet.Cells[row, 5].Value = product.Price;
                    row++;
                }

                // Автоматическая подгонка ширины столбцов
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Добавляем диаграмму
                ExcelChart chart = worksheet.Drawings.AddChart("PriceChart", eChartType.ColumnClustered);
                chart.Title.Text = "Product Prices";
                chart.SetPosition(row, 0, 0, 0);  // Позиция диаграммы
                chart.SetSize(600, 400);  // Размер диаграммы

                // Данные для диаграммы (цены продуктов)
                ExcelRange priceRange = worksheet.Cells[4, 5, row - 1, 5];
                ExcelRange nameRange = worksheet.Cells[4, 2, row - 1, 2];

                // Добавление данных в диаграмму
                ExcelChartSerie series = chart.Series.Add(priceRange, nameRange);
                series.Header = "Prices";

                // Сохраняем файл
                package.Save();
            }

            _ = Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });

            _ = MessageBox.Show("Отчет в Excel создан!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnGenRepInWord_Click(object sender, RoutedEventArgs e)
        {
            GenerateWordReport(result);
        }

        private void btnGenRepInExcel_Click(object sender, RoutedEventArgs e)
        {
            GenerateExcelReport(result);
        }

        private void LoadChartData()
        {
            // Получаем данные из базы данных
            List<Products> categories = new DB_Operation().ExecuteQuery<Products>(@"SELECT * FROM Categories;"); ;
            IEnumerable<Products> products = result;

            // Подготавливаем данные для графика
            var categoryPrices = categories.Select(c => new
            {
                CategoryName = c.Name,
                AveragePrice = products.Where(p => p.ID_Category == c.ID_Category)
                                       .Select(p => p.Price)
                                       .DefaultIfEmpty(0) // Если нет товаров в категории, средняя цена = 0
                                       .Average()
            }).ToList();

            // Устанавливаем категории по оси X
            Categories = categoryPrices.Select(c => c.CategoryName).ToArray();

            // Формируем столбцы для каждой категории
            SeriesCollection =
            [
                new ColumnSeries
                {
                    Title = "Средняя цена",
                    Values = new ChartValues<double>(categoryPrices.Select(c => c.AveragePrice))
                }
            ];

            // Форматирование оси Y для отображения цен в рублях
            Formatter = value => value.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartData();
            DataContext = this;
        }
    }
}
