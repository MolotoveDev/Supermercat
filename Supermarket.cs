using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Dictionary<string, Person> staff;
        Dictionary<string, Person> customers;
        SortedDictionary<int, Item> warehouse;
        const string fileItems = "GROCERIES.txt";
        const string fileCashiers = "CASHIERS.txt";
        const string fileCustomers = "CUSTOMERS.txt";

        #endregion

        #region Constructor

        public Supermarket(string name, string address, string fileCashiers, string fileCustomers, string fileItems, int activeLines)
        {
            LoadCustomers(fileCustomers);
            LoadStaff(fileCashiers);
            LoadItems(fileItems);
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
        }

        private Dictionary<string, Person> LoadStaff(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
        }

        /// <summary>
        /// Method to load the items from a file.
        /// </summary>
        /// <param name="fileName">Name of the file to load the items.</param>
        /// <returns>Sorted dictionary with the warehouse items.</returns>
        private SortedDictionary<int, Item> LoadItems(string fileName)
        {
            //Setting the attributes
            StreamReader sr = new StreamReader(fileName);
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
            Random r = new Random(); //Random to create the stock and minStock values + sale value.

            line = sr.ReadLine();
            while (line != null)
            {
                split = line.Split(';');
                name = split[0];
                category = Enum.Parse<Item.Category>(split[1], true); //Conversion of string to enum
                format = Enum.Parse<Item.Packaging>(split[2], true); //Conversion of string to enum
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
