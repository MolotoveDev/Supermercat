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
        
        private string name;
        private string address;
        public static int MAXLINES = 5;
        private int activeLines;
        CheckOutLine[] lines = new CheckOutLine[MAXLINES];
        Dictionary<string, Person> cashiers;
        Dictionary<string, Person> customers;
        SortedDictionary<int, Item> warehouse;

        #endregion

        #region Constructor

        public Supermarket(string name, string address, string fileCashiers, string fileCustomers, string fileItems, int activeLines)
        {
            this.name = name;
            this.address = address;
            LoadCustomers(fileCustomers);
            LoadCashiers(fileCashiers);
            LoadWarehouse(fileItems);
            this.activeLines = activeLines;
            for (int i = 0; i < activeLines - 1; i++)
            {
                lines[i] = new CheckOutLine(GetAvaibleCashier(), i + 1);
            }
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

        /// <summary>
        /// Property to get a reference to the warehouse (dictionary of items).
        /// </summary>
        public SortedDictionary<int, Item> Warehouse
        {
            get { return warehouse; }
        }

        /// <summary>
        /// Property to get a reference to the staff (dictionary of cashiers).
        /// </summary>
        public Dictionary<string, Person> Staff
        {
            get { return cashiers; }
        }

        /// <summary>
        /// Property to get a reference to the customers (dictionary of customers).
        /// </summary>
        public Dictionary<string, Person> Customers
        {
            get { return customers; }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Method to load the customers from a file.
        /// </summary>
        /// <param name="fileName">Name of the file to load.</param>
        /// <returns>Dictionary with customers information</returns>
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

        /// <summary>
        /// Method to load the cashiers from a file.
        /// </summary>
        /// <param name="fileName">Name of the file to load.</param>
        /// <returns>Dictionary with Cashiers information</returns>
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

                if (r.Next(0, 20) >= 15) sale = true;

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

        /// <summary>
        /// Method to get the items in the warehouse sorted by stock from lowest to highest.
        /// </summary>
        /// <returns>Sorted set with warehouse items sorted by smallest to largest stock.</returns>
        public SortedSet<Item> GetItemsByStock()
        {
            SortedSet<Item> items = new SortedSet<Item>();
            foreach (Item i in warehouse.Values)
            {
                items.Add(i);
            }
            items.OrderBy(i => i.Stock);
            return items;
        }

        /// <summary>
        /// Method to get any avaliable customer from the customers dictionary.
        /// </summary>
        /// <returns>A avaliabable customer.</returns>
        public Person GetAvailableCustomer()
        {
            int index = 0;
            bool found = false;
            Person person = null;
            
            while(index < customers.Count && !found)
            {
                if(!customers.ElementAt(index).Value.Active)
                {
                    found = true;
                    person = customers.ElementAt(index).Value;
                }
                else
                {
                    index++;
                }
            }
            return person;
        }

        /// <summary>
        /// Method to get any avaliable cashier from the cashiers dictionary.
        /// </summary>
        /// <returns>A availabable cashier.</returns>
        public Person GetAvaibleCashier()
        {
            int index = 0;
            bool found = false;
            Person person = null;

            while (index < cashiers.Count && !found)
            {
                if (!cashiers.ElementAt(index).Value.Active)
                {
                    found = true;
                    person = cashiers.ElementAt(index).Value;
                }
                else
                {
                    index++;
                }
            }
            return person;
        }

        /// <summary>
        /// Method to add an additional line to the supermarket.
        /// </summary>
        /// <param name="line2Open">Number of the line to open.</param>
        /// <exception cref="ArgumentException">Exception if the line to open is bigger than the MAXLINES or the number of active lines is
        /// at the limit or if the line2Open is already opened.</exception>
        public void OpenCheckOutLine(int line2Open)
        {
            if (activeLines >= MAXLINES || line2Open > MAXLINES) throw new ArgumentException("ERROR: No hi ha prou caixes disponibles");
            if (lines.Length >= line2Open) throw new ArgumentException("ERROR: La caixa ja esta oberta");
            lines[line2Open - 1] = new CheckOutLine(GetAvaibleCashier(), line2Open);
            activeLines++;
        }

        /// <summary>
        /// Method to get the object of the CheckOutLine class.
        /// </summary>
        /// <param name="lineNumber">Line number to get the object from</param>
        /// <returns>Object of CheckOutLine based on the lineNumber or null if it doesn't exist.</returns>
        public CheckOutLine? GetCheckOutLine(int lineNumber)
        {
            CheckOutLine? line = null;
            line = lines[lineNumber - 1];
            return line;
        }

        /// <summary>
        /// Method to enqueue a shopping cart to the line.
        /// </summary>
        /// <param name="theCart">Cart to add to the line.</param>
        /// <param name="line">Line the cart will be added.</param>
        /// <returns>True if gets added, false if not.</returns>
        public bool JoinTheQueue(ShoppingCart theCart, int line)
        {
            bool result = false;
            if (lines[line] != null)
            {
                lines[line].CheckIn(theCart);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method to dequeue a shopping cart from the line.
        /// </summary>
        /// <param name="line">Line to dequeue the shopping cart from.</param>
        /// <returns>True if the line exist and gets dequeued, false if not.</returns>
        public bool Checkout(int line)
        {
            bool result = false;
            if (lines[line] != null)
            {
                lines[line].CheckOut();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Method toString to show the supermarket information.
        /// </summary>
        /// <returns>String with the supermarket information.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.name);
            sb.AppendLine(this.address);
            for (int i = 0; i < lines.Length; i++)
            {
                sb.AppendLine(lines[i].ToString());
            }
            return sb.ToString();
        }

        #endregion
    }
}