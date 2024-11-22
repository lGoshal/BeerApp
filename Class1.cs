using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BeerApp
{
    public partial class BeerRepository : MainWindow
    {
        private string _connectionString;

        public BeerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Метод для получения всех записей из таблицы пива
        public DataTable GetAllBeers()
        {
            DataTable beersTable = new DataTable();
            string query = @"SELECT b.BeerID, b.Name, bt.TypeName AS BeerType, m.Name AS Manufacturer, b.ABV, b.Price, b.ProductionDate
                         FROM Beer b
                         JOIN BeerType bt ON b.BeerTypeID = bt.BeerTypeID
                         JOIN Manufacturer m ON b.ManufacturerID = m.ManufacturerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(beersTable);
            }
            return beersTable;
        }

        // Метод для добавления новой записи
        public void AddBeer(string name, int beerTypeId, int manufacturerId, decimal abv, decimal price, DateTime productionDate)
        {
            string query = "INSERT INTO Beer (Name, BeerTypeID, ManufacturerID, ABV, Price, ProductionDate) " +
                           "VALUES (@Name, @BeerTypeID, @ManufacturerID, @ABV, @Price, @ProductionDate)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@BeerTypeID", beerTypeId);
                command.Parameters.AddWithValue("@ManufacturerID", manufacturerId);
                command.Parameters.AddWithValue("@ABV", abv);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@ProductionDate", productionDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Метод для редактирования существующей записи
        public void UpdateBeer(int beerId, string name, int beerTypeId, int manufacturerId, decimal abv, decimal price, DateTime productionDate)
        {
            string query = "UPDATE Beer SET Name=@Name, BeerTypeID=@BeerTypeID, ManufacturerID=@ManufacturerID, ABV=@ABV, Price=@Price, ProductionDate=@ProductionDate " +
                           "WHERE BeerID=@BeerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BeerID", beerId);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@BeerTypeID", beerTypeId);
                command.Parameters.AddWithValue("@ManufacturerID", manufacturerId);
                command.Parameters.AddWithValue("@ABV", abv);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@ProductionDate", productionDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Метод для удаления записи
        public void DeleteBeer(int beerId)
        {
            string query = "DELETE FROM Beer WHERE BeerID=@BeerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BeerID", beerId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
