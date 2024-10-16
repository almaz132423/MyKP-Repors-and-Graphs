Category Price Reports and Charts
This project is designed to generate reports on the average prices of products by category and visualize this data using a bar chart with LiveCharts.Wpf. The data is fetched from an SQLite database, and reports are generated in a text format, while the chart is displayed in a WPF application.

Features
Text Report Generation – Generates reports showing the average prices of products by category, and saves them as text files on the desktop.
Data Visualization – Displays the average prices of products by category using a bar chart in a WPF application.
SQLite Integration – Fetches category and product data from an SQLite database.
Installation and Setup
Step 1: Clone the repository
bash
Копировать код
git clone https://github.com/your_username/CategoryPriceReports.git
cd CategoryPriceReports
Step 2: Install dependencies
The project uses LiveCharts.Wpf for charting. Install it using NuGet Package Manager:

bash
Копировать код
Install-Package LiveCharts.Wpf
Step 3: Set up the SQLite database
Ensure that you have an SQLite database set up with the necessary tables and data. You can use the following SQL to create the tables and insert some initial data:

sql
Копировать код
CREATE TABLE "Categories" (
    "ID_Category" INTEGER NOT NULL,
    "Name" TEXT,
    PRIMARY KEY("ID_Category" AUTOINCREMENT)
);

CREATE TABLE "Products" (
    "ID_Product" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Image" TEXT NOT NULL,
    "ID_Category" INTEGER NOT NULL,
    "Price" REAL NOT NULL,
    PRIMARY KEY("ID_Product" AUTOINCREMENT),
    FOREIGN KEY("ID_Category") REFERENCES "Categories"("ID_Category")
);

INSERT INTO Categories (Name)
VALUES
('Electronics'), ('Books'), ('Clothing'), ('Home Appliances'), ('Toys'),
('Beauty & Care'), ('Sport Equipment'), ('Automotive Products'), ('Stationery'), ('Food');

INSERT INTO Products (Name, Description, Image, ID_Category, Price)
VALUES
('Smartphone', 'A powerful smartphone with a great camera', 'image_url', 1, 19999.99),
('Laptop', 'High-performance laptop for work and gaming', 'image_url', 1, 59999.99),
('Book "War and Peace"', 'Classic literature', 'image_url', 2, 499.99);
Step 4: Run the project
You can run the project in Visual Studio or via the command line:

bash
Копировать код
dotnet run
Step 5: Generate Reports
The text report will be saved on your desktop as CategoryPricesReport.txt, containing the average prices for each category.
The chart will be displayed in the WPF application, showing the categories on the X-axis and the average price on the Y-axis.
Project Structure
MainWindow.xaml – The XAML layout for the bar chart visualization.
MainWindow.xaml.cs – Code-behind for data loading, average price calculation, and chart binding.
DataModels.cs – Defines Product and Category models.
SQLiteHelper.cs – Helper methods for working with the SQLite database.
Example Text Report
The generated report will look like this:

yaml
Копировать код
Category Price Report:
-------------------------------
Category: Electronics, Average Price: 39,999.99 RUB
Category: Books, Average Price: 499.99 RUB
Category: Clothing, Average Price: 799.99 RUB
...
-------------------------------
Report generated on: 16.10.2024
Example Chart
The bar chart displays the average price of products by category:

X-axis – Categories.
Y-axis – Average price of products in RUB.
(replace this with your actual chart image)

Technologies Used
WPF – Used to create the graphical interface.
LiveCharts.Wpf – Library for interactive charting.
SQLite – Local database used to store product and category data.
Contact
For questions or suggestions, feel free to contact me at your.email@example.com.

Note:
Make sure to update the actual chart image and SQLite database details based on your real setup. This README.md provides an organized and informative structure for users visiting your GitHub repository.
