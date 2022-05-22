using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse
{
    internal class Product
    {
        public int id { get; set; }

        public string name;

        public string unit;

        public int price;

        public string date_of_last_delivery;

        public int amount;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Date_of_last_delivery
        {
            get { return date_of_last_delivery; }
            set { date_of_last_delivery = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Product () { }

        public Product(string name, string unit, int price, string date_of_last_delivery, int amount)
        {
            this.name = name;
            this.unit = unit;
            this.price = price;
            this.date_of_last_delivery = date_of_last_delivery;
            this.amount = amount;
        }

    }
}
