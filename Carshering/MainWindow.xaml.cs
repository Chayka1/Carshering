using System.Windows;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Controls;

namespace CarSharing
{
    public partial class MainWindow : Window
    {
        private List<Car> cars;
        private SQLiteConnection connection;

        public MainWindow()
        {
            InitializeComponent();
            cars = new List<Car>();
            connection = new SQLiteConnection("Data Source=Cars.sqlite;Version=3;");
            connection.Open();
            string createTableQuery = "CREATE TABLE IF NOT EXISTS Cars (Brand TEXT, Model TEXT, PricePerHour INTEGER)";
            SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection);
            createTableCommand.ExecuteNonQuery();
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            string brand = BrandTextBox.Text;
            string model = ModelTextBox.Text;
            int price_per_hour;
            if (!int.TryParse(PriceTextBox.Text, out price_per_hour))
            {
                MessageBox.Show("Введите целочисленное значение в поле 'Цена за час'");
                return;
            }
            Car car = new Car(brand, model, price_per_hour);
            cars.Add(car);
            string insertQuery = $"INSERT INTO Cars (Brand, Model, PricePerHour) VALUES ('{brand}', '{model}', {price_per_hour})";
            SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
            insertCommand.ExecuteNonQuery();
            BrandTextBox.Clear();
            ModelTextBox.Clear();
            PriceTextBox.Clear();
        }

        private void TakeACarButton_Click(object sender, RoutedEventArgs e)
        {
            string brand = BrandTextBox.Text;
            string model = ModelTextBox.Text;
            foreach (Car car in cars)
            {
                if (car.Brand == brand && car.Model == model)
                {
                    MessageBox.Show($"Стоимость данной машины в час составит {car.PricePerHour}$");
                    cars.Remove(car);
                    string deleteQuery = $"DELETE FROM Cars WHERE Brand='{brand}' AND Model='{model}'";
                    SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection);
                    deleteCommand.ExecuteNonQuery();
                    return;
                }
            }
            MessageBox.Show("Машину не найдено. Введите существующую модель!");
        }

        private void ListOfAvailableCarsButton_Click(object sender, RoutedEventArgs e)
        {
            string selectQuery = "SELECT Brand, Model, PricePerHour FROM Cars";
            SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection);
            SQLiteDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                string brand = reader.GetString(0);
                string model = reader.GetString(1);
                int price_per_hour = reader.GetInt32(2);
                Car car = new Car(brand, model, price_per_hour);
                cars.Add(car);
                AvailableCarsTextBox.Text += $"{car.Brand}, {car.Model} : {car.PricePerHour}$/час\n";
            }
        }
    }
}
