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

        private SortedDictionary<int, Item> LoadItems(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            //name;category;format;priceEach
            string[] split;
            string line;
            string name, category, format;
            double priceEach;

            line = sr.ReadLine();
            while(line != null)
            {
                split = line.Split(';');
                name = split[0];
                category = split[1];
                format = split[2];
                priceEach = double.Parse(split[3]);
                Item item = new Item(name, category, format, priceEach);
            }
        }

        #endregion
    }
}
