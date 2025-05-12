using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Supermercat.Person;

namespace Supermercat
{
    public class Supermarket
    {
        #region Attributes
        
        string name;
        string address;
        public static int MAXLINES = 5;
        int activeLines;
        //TODO: Add checkout class.
        // CheckOutLine[] lines = new CheckOutLine[MAXLINES];
        Dictionary<string, Person> cashiers;
        Dictionary<string, Person> customers;
        SortedDictionary<int, Item> warehouse;


        #endregion

        #region Constructor

        public Supermarket(string name, string address, string fileCashiers, string fileCustomers, string fileItems, int activeLines)
        {
            this.name = name;
            this.address = address;
            this.activeLines = activeLines;
            LoadCustomers(fileCustomers);
            LoadCashiers(fileCashiers);
            LoadWarehouse(fileItems);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property to know the number of active lines in the supermarket.
        /// </summary>
        public int ActiveLines
        {
            get { return activeLines; }
        }

        #endregion

        #region Methods

        private Dictionary<string, Person> LoadCustomers(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            string line;
            string[] split;
            customers = new Dictionary<string, Person>();
            line = sr.ReadLine();

            while (line != null)
            {
                split = line.Split(';');
                if (split[2] != "")
                {
                    Person customer = new Customer(split[0], split[1], Convert.ToInt32(split[2]));
                    customers.Add(split[0], customer);
                }
                else 
                { 
                    Person customer = new Customer(split[0], split[1], null);
                    customers.Add(split[0], customer);
                }
                line = sr.ReadLine();
            }
            sr.Close();
            return customers;
        }

        private Dictionary<string, Person> LoadCashiers(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            cashiers = new Dictionary<string, Person>();
            string line;
            string[] split;
            line = sr.ReadLine();

            while (line != null)
            {
                split = line.Split(';');
                Person cashier = new Cashier(split[0], split[1], Convert.ToDateTime(split[3]));
                cashiers.Add(split[0], cashier);
                line = sr.ReadLine();
            }
            sr.Close();
            return cashiers;
        }

        /// <summary>
        /// Method to load the items from a file.
        /// </summary>
        /// <param name="fileName">Name of the file to load the items.</param>
        /// <returns>Sorted dictionary with the warehouse items.</returns>
        private SortedDictionary<int, Item> LoadWarehouse(string fileName)
        {
            //Setting the attributes
            StreamReader sr = new StreamReader(fileName);
            warehouse = new SortedDictionary<int, Item>();
            int code = 1;
            string[] split;
            string line;
            string name;
            Item.Category category;
            Item.Packaging format;
            double priceEach;
            bool sale = false;
            double stock;
            int minStock;
            string valor = "";
            Random r = new Random(); //Random to create the stock and minStock values + sale value.

            line = sr.ReadLine();
            while (line != null)
            {
                split = line.Split(';');
                name = split[0];
                category = Enum.Parse<Item.Category>(split[1], true); //Conversion of string to enum
                if (split[2] == "K") { valor = "Kg"; }
                else if (split[2] == "U") { valor = "Unit"; }
                else if (split[2] == "P") { valor = "Package"; }
                format = Enum.Parse<Item.Packaging>(valor, true); //Conversion of string to enum
                priceEach = double.Parse(split[3]);

                if (r.Next(0, 20) >= 10) sale = true;

                stock = r.Next(50, 125);
                minStock = r.Next(10, 49);

                Item item = new Item(code, name, sale, priceEach, category, format, stock, minStock); //Constructor for the items
                warehouse.Add(code, item);
                code++;
                line = sr.ReadLine();
            }
            sr.Close();
            return warehouse;
        }
        #endregion
    }
}
 