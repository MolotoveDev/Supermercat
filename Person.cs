using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Supermercat.Person;

namespace Supermercat
{
    public abstract class Person : IComparable<Person>
    {
        private string _fullName;
        private string _id;
        private int _points;
        private double _totalInvoiced;
        private bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        public string FullName
        {
            get { return _fullName; }
        }
        public abstract double GetRating { get; }
        /// <summary>
        /// First constructor.
        /// </summary>
        /// <param name="id">ID to be added</param>
        /// <param name="fullName">fulName to be added</param>
        /// <param name="points">point to be added</param>
        protected Person(string id, string fullName, int points)
        {
            _id = id;
            _fullName = fullName;
            _points = points;
            _totalInvoiced = 0;
            active = false;
        }
        /// <summary>
        /// additional constructor (point will be 0).
        /// </summary>
        /// <param name="id">ID to be added</param>
        /// <param name="fullName">fulName to be added</param>
        protected Person(string id, string fullName) : this(id, fullName, 0)
        {
        }
        /// <summary>
        /// Method useful to add the amount invoiced in a purchase operation
        /// </summary>
        /// <param name="amount">amount to be added</param>
        public void AddInvoiceAmount(double amount)
        {
            _totalInvoiced += amount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns a string with S (if available) and N (if not available).</returns>
        public override string ToString()
        {
            string result;
            if (Active) { result = "N"; }
            else { result = "S"; }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsToAdd"></param>
        public abstract void AddPoints(int pointsToAdd);

        public int CompareTo(Person? other)
        {
            return this.GetRating.CompareTo(other.GetRating);
        }

        public class Customer : Person
        {
            private int? _fidelity_card;

            /// <summary>
            /// Creates a new customer initializing the base class attributes + fidelity card attribute.
            /// </summary>
            /// <param name="id">Id from the new costumer</param>
            /// <param name="fullName">fullName from the new costumer</param>
            /// <param name="fidelityCard">fidelity card from the new costumer</param>
            public Customer(string id, string fullName, int? fidelityCard) : base(id, fullName)
            {
                _fidelity_card = fidelityCard;
                _id = id;
                _fullName = fullName;
            }
            /// <summary>
            /// The property calculates the rating of a customer according to the gross amount 
            /// of his/her purchases.The rating is the 2% of the customer’s purchases
            /// </summary>
            public override double GetRating
            {
                get
                {
                    double resposta = 0;
                    if (_totalInvoiced != 0)
                    {
                        resposta = (2 / _totalInvoiced) *100;
                    }
                    return resposta;
                }

            }


            /// <summary>
            /// adds the points parameter to the current points of the customer 
            /// except if the customer hasn't a fidelity card.In this case points are lost (no added to anything)
            /// </summary>
            /// <param name="pointsToAdd">points to add</param>
            public override void AddPoints(int pointsToAdd)
            {
                if(_id != "CASH")
                {
                    _points += pointsToAdd;
                }
            }
            public override string ToString()
            {
                return $"DNI/NIE-> {_id} NOM-> {_fullName} RATING-> {GetRating} vendes-> {_totalInvoiced}€ PUNTS->{_points} DISPONIBLE-> {base.ToString()}";
            }
            public override bool Equals(object? obj)
            {
                bool igual;
                if (ReferenceEquals(null, obj)) igual = false;
                else if (ReferenceEquals(this, obj)) igual = true;
                else if (obj.GetType() != this.GetType()) igual = false;
                else igual = this.Equals((Customer)obj);
                return igual;
            }
            private bool Equals(Customer other)
            {
               return this.active == other.active;
            }
        }
        public class Cashier : Person
        {
           
            private DateTime _joiningDate;
            /// <summary>
            /// the complete years of service in the supermarket the cashier was hired.
            /// </summary>
            public int YearOfService
            {
                get
                {
                    return (DateTime.Today - _joiningDate).Days / 365;
                }
            }
            /// <summary>
            /// Creates a new cashier initializing the base class attributes + joining date attribute.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="fullName"></param>
            /// <param name="joining_Date"></param>
            public Cashier(string id, string fullName,DateTime joining_Date) : base(id, fullName)
            {
                _joiningDate = joining_Date;
                _id = id;
                _fullName = fullName;
            }
            /// <summary>
            /// 
            /// </summary>
            public override double GetRating => (10 / _joiningDate.Day) * 100;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="pointsToAdd"></param>
            public override void AddPoints(int pointsToAdd)
            {
               _points += (YearOfService + 1) * pointsToAdd;
            }
            public override string ToString()
            {
                return $"DNI/NIE-> {_id} NOM-> {_fullName} RATING-> {GetRating} ANTIGUITAT-> {YearOfService} VENDES-> {_totalInvoiced}€ PUNTS->{_points} DISPONIBLE-> {base.ToString()}";
            }
        }
    }
}
