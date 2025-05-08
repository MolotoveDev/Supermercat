using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermercat
{
    public class Item : IComparable<Item>
    {
        #region Atributes

        public enum Category 
        { BEVERAGE = 1, FRUITS, VEGETABLES, BREAD, MILK_AND_DERIVATES, GARDEN, MEAT, SWEETS, SAUCES, FROZEN, CLEANING, FISH, OTHER };
        public enum Packaging { Unit, Kg, Package };
        
        //List of attributes used for this class.

        char currency = '\u20AC'; // Euro (€) sign
        int code;
        string description;
        bool onSale;
        double price;
        Category category;
        Packaging packaging;
        double stock;
        int minStock;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the Item class.
        /// </summary>
        /// <param name="code">Code for the item.</param>
        /// <param name="description">Description for the item to add.</param>
        /// <param name="onSale">Is the item on sale.</param>
        /// <param name="price">Price of the item.</param>
        /// <param name="category">Category of the item.</param>
        /// <param name="packaging">Packaging of the item.</param>
        /// <param name="stock">Stock of the item.</param>
        /// <param name="minStock">Minimum stock for the item.</param>
        /// <exception cref="ArgumentException">Exception if the introduced price is negative.</exception>
        /// <exception cref="ArgumentNullException">Exception if the introduced description is empty.</exception>"
        public Item(int code, string description, bool onSale, double price, Category category, Packaging packaging, double stock, int minStock)
        {
            if (price < 0) throw new ArgumentException("Price cannot be negative.");
            if (description == "" || description == null) throw new ArgumentNullException("Description cannot be empty.");
            this.code = code;
            this.description = description;
            this.onSale = onSale;
            this.price = price;
            this.category = category;
            this.packaging = packaging;
            this.stock = stock;
            this.minStock = minStock;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Propertry to get the description of any item.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Property to get the category of any item.
        /// </summary>
        public string getCaregory
        {
            get { return category.ToString(); }
        }

        /// <summary>
        /// Property to get the minimum stock of any item.
        /// </summary>
        public int MinStock
        {
            get { return minStock; }
        }

        /// <summary>
        /// Property to know if the item is on sale.
        /// </summary>
        public bool OnSale
        {
            get { return onSale; }
        }

        /// <summary>
        /// Property to get the packaging type of any item.
        /// </summary>
        public string PackagingType
        {
            get { return packaging.ToString(); }
        }

        /// <summary>
        /// Property to get the price of any item.
        /// </summary>
        public double Price
        {
            get 
            { 
                if(OnSale)
                {
                    return price * 0.9; // 10% discount
                }
                else
                {
                    return price;
                }
            }
        }

        /// <summary>
        /// Property to get the actual stock of any item.
        /// </summary>
        public double Stock
        {
            get { return stock; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to update the stock of any item.
        /// </summary>
        /// <param name="stock">Updated stock</param>
        public void UpdateStock(Item item, double qty)
        {
            item.stock = qty;
        }

        /// <summary>
        /// Method ToString any item information.
        /// </summary>
        /// <returns>string text with the item information.</returns>
        public override string ToString()
        {
            string output;
            if(onSale)
            {
                output = $"CODE->{this.code} DESCRIPTION->{this.description} CATEGORY ->{this.category} STOCK ->{this.stock} PRICE->{this.price}{currency} ON SALE ->Y({this.Price}){currency}";
            }
            else
            {
                output = $"CODE->{this.code} DESCRIPTION->{this.description} CATEGORY ->{this.category} STOCK ->{this.stock} PRICE->{this.price}{currency} ON SALE ->N";
            }
            return output;
        }

        #endregion

        #region Icomparable<Item> Members

        /// <summary>
        /// Method to compare two items.
        /// </summary>
        /// <param name="other">Other item to compare.</param>
        /// <returns>Comparable result to sort items.</returns>
        public int CompareTo(Item? other)
        {
            return this.code.CompareTo(other.code);
        }
        
        #endregion
    }
}
