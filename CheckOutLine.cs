using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Supermercat
{
    public class CheckOutLine
    {
        #region Attributes
        private int _number;
        private Queue<ShoppingCart> queue;
        private Person cashier;
        private bool active;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor from CheckOutLine class, create a new line.
        /// </summary>
        /// <param name="responsible">the cashier who will be on the line.</param>
        /// <param name="number">the line number</param>
        public CheckOutLine(Person responsible, int number)
        {
            _number = number;
            cashier = responsible;
            active = true;
            queue = new Queue<ShoppingCart>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property to know if ther's any shopping cart in the line.
        /// </summary>
        public bool Empty
        {
            get
            {
                bool result = false;
                if(queue.Count == 0) result = true;
                return result;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// method tries to enqueue the shopping cart if the line is active.
        /// </summary>
        /// <param name="oneShoppingCart">the shopping cart you want to enqueue</param>
        /// <returns></returns>
        public bool CheckIn(ShoppingCart oneShoppingCart)
        {
            bool result = false; 
            if(active)
            {
                queue.Enqueue(oneShoppingCart);
                result = true;
            }
            return result;
        }
        /// <summary>
        /// method dequeues the Shopping cart that is at the first position of the line (only if the line is active and have some shoppingcart)
        /// </summary>
        /// <returns>returns true if do the dequeue to shoppingcart at the first position.</returns>
        public bool CheckOut()
        {
            bool result = false;
            if (queue.Count > 0 && active == true)
            {
                ShoppingCart next = queue.Dequeue();
                double suma = 0;
                int points;

                foreach (KeyValuePair<Item, double> product in next.ShoppingList)
                {
                    suma += product.Key.Price * product.Value;
                }
                next.Customer.AddInvoiceAmount(suma);
                this.cashier.AddInvoiceAmount(suma);

                points = next.RawPointsObtainedAtCheckout(suma);
                next.Customer.AddPoints(points);
                this.cashier.AddPoints(points);

                next.Customer.Active = false;
                result = true;
            }
            return result;
        }
        /// <summary>
        /// ToString method of line.
        /// </summary>
        /// <returns>returns ToString like string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"NUMERO DE CAIXA --> {_number}");
            sb.AppendLine($"CAIXER/A AL CÀRREC --> {cashier.FullName}");
            sb.AppendLine("*******");
            foreach(ShoppingCart cart in queue)
            {
                sb.Append(cart.ToString());
            }
            return sb.ToString();
        }
        #endregion
    }
}
