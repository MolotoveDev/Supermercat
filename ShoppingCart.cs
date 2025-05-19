using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Supermercat.Person;

namespace Supermercat
{
    public class ShoppingCart
    {
        #region Attributes

        private Dictionary<Item, double> shoppingList;
        private Customer customer;
        private DateTime dateOfPurchase = DateTime.Now;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the ShoppingCart class.
        /// </summary>
        /// <param name="customer">Customer owener of the shopping cart.</param>
        /// <param name="dateOfPurchase">Date of the purchase</param>
        public ShoppingCart(Customer customer, DateTime dateOfPurchase)
        {
            this.customer = customer;
            this.dateOfPurchase = dateOfPurchase;
            this.shoppingList = new Dictionary<Item, double>();
            customer.Active = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property to get the shopping list.
        /// </summary>
        public Dictionary<Item, double> ShoppingList
        {
            get { return shoppingList; }
        }

        /// <summary>
        /// Property to get the customer.
        /// </summary>
        public Customer Customer
        {
            get { return customer; }
        }

        /// <summary>
        /// Property to get the date of purchase.
        /// </summary>
        public DateTime DateOfPurchase
        {
            get { return dateOfPurchase; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to add or update an item to the shopping list.
        /// </summary>
        /// <param name="item">Item to add or update.</param>
        /// <param name="qty">New quantity to add</param>
        /// <exception cref="ArgumentException">Exception to check that qty is correct depending of the packaging and stock.</exception>
        public void AddOne(Item item, double qty)
        {
            //Exception Control
            if((item.PackagingType == "Unit" || item.PackagingType == "Package") && qty % 1 != 0)
            {
                throw new ArgumentException("ERROR: En cas d'unitat o paquet la quantitat ha de ser un enter");
            }
            if (item.Stock < qty) throw new ArgumentException("ERROR: No hi ha prou stock");
            
            //Add or update check + procedure 
            if(shoppingList != null && shoppingList.ContainsKey(item)) shoppingList[item] += qty; //If the item is already in the list, update the quantity
            else shoppingList.Add(item, qty); //If the item is not in the list, add it with the quantity.
        }

        /// <summary>
        /// Method to add a random number of items (between 1 and 10) to the shopping list.
        /// </summary>
        /// <param name="warehouse">warehouse where the item will get added.</param>
        public void AddAllRandomly(SortedDictionary<int, Item> warehouse)
        {
            Random r = new Random();
            for (int i = 1; i <= r.Next(1, 10); i++)
            {
                shoppingList.Add(warehouse.ElementAt(r.Next(0, warehouse.Count)).Value, r.Next(1, 5)); //Add a random item from the warehouse to the shopping list with a random quantity between 1 and 5.
            }
        }

        /// <summary>
        /// Method that return the total points gained by the customer on this purchase.
        /// </summary>
        /// <param name="totalInvoiced">Total spent by the customer on this purchase</param>
        /// <returns>Integer number of points.</returns>
        public int RawPointsObtainedAtCheckout(double totalInvoiced)
        {
            return (int)Math.Round(totalInvoiced * 0.01);
        }

        /// <summary>
        /// Method to process the items in the shopping list and update the stock of the items in the warehouse.
        /// </summary>
        /// <param name="cart">Cart where the items will get processed.</param>
        /// <returns>Double with the total the client must pay</returns>
        public static double ProcessItems(ShoppingCart cart)
        {
            //WARNING: This method is a bit spaghetti code, probablly it could be improved and fixed with a better structure.
            double total = 0;
            for (int i = 0; i <= cart.ShoppingList.Count - 1; i++)
            {
                if (cart.shoppingList.Values.ElementAt(i) > cart.ShoppingList.ElementAt(i).Key.Stock) //If the quantity in the shopping list is greater than the stock, set it to the stock value.
                {
                    cart.ShoppingList[cart.ShoppingList.ElementAt(i).Key] = cart.ShoppingList.ElementAt(i).Key.Stock; //Set the quantity to the stock value.
                    cart.ShoppingList.ElementAt(i).Key.UpdateStock(cart.ShoppingList.ElementAt(i).Key, 0); //Update the stock of the item in the warehouse.
                    Console.WriteLine("ATENCIÓ: No hi ha pro stock per a l'element " + cart.ShoppingList.ElementAt(i).Key.Description + ". S'ha ajustat la quantitat a " + cart.ShoppingList.ElementAt(i).Key.Stock + "."); //Notify the user
                }
                else
                {
                    cart.ShoppingList.ElementAt(i).Key.UpdateStock(cart.ShoppingList.ElementAt(i).Key, cart.ShoppingList.ElementAt(i).Key.Stock - cart.ShoppingList.Values.ElementAt(i)); //Update the stock of the item in the warehouse.
                }
                total += cart.ShoppingList.ElementAt(i).Key.Price * cart.ShoppingList.Values.ElementAt(i); //Calculate the total price of the shopping list.
            }
            return Math.Round(total, 2); //Return the total price of the shopping list.
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("**************");
            sb.AppendLine("INFO CARRITO DE COMPRA CLIENT -> " + customer.FullName);
            foreach (Item i in shoppingList.Keys)
            {
                if(i.OnSale)
                {
                    sb.AppendLine(i.Description + "      " + "CAT--->" + i.getCaregory + "      " + "QTY-->" + shoppingList[i] + "      " + "UNIT PRICE--->" + Math.Round(i.Price, 2) + "\u20AC" + "(*)");
                }
                else
                {
                    sb.AppendLine(i.Description + "      " + "CAT--->" + i.getCaregory + "      " + "QTY-->" + shoppingList[i] + "      " + "UNIT PRICE--->" + Math.Round(i.Price, 2) + "\u20AC");
                }
            }
            sb.AppendLine("**************FI CARRITO COMPRA**************");
            return sb.ToString();
        }

        #endregion
    }
}
