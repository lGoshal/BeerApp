using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerApp
{
    internal class BeerCon
    {
        public int BeerID { get; set; }
        public string Name { get; set; }
        public int BeerTypeID { get; set; }
        public int ManufacturerID { get; set; }
        public double ABV { get; set; }
        public double Price { get; set; }
        public string ProductDate { get; set; }
        public string Link { get; set; }
    }
    internal class QueryBeer
    {
        public int BeerID { get; set; }
        public string Name { get; set; }
        public int BeerTypeID { get; set; }
        public int ManufacturerID { get; set; }
        public double ABV { get; set; }
        public double Price { get; set; }
        public string ProductDate { get; set; }
    }

    internal class BeerIngredients
    {
        public int BeerID { get; set; }
        public int IngredientID { get; set; }
        public decimal Quantity { get; set; }
    }

    internal class BeerTypes
    {
        public int BeerTypeID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }

    internal class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    internal class Ingridient
    {
        public int IngredientID { get; set; }
        public string Name { get; set; }
        public string Supplier { get; set; }
        public string Unit {  get; set; }
        public string CostPerUnit { get; set; }
    }

    internal class Manufacturers
    {
        public int ManufacturerID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public int EstablishedYear { get; set; }
    }

    internal class Orders
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

    internal class OrdersBeer
    {
        public int OrderID { get; set; }
        public int BeerID { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }

    public class ProductionsProcess
    {
        public int ProcessID { get; set; }
        public int BeerID { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; }
        public decimal DurationHours { get; set; }
    }
    internal class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int CustomerID { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
