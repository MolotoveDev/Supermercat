using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Supermercat
{
    public abstract class Person : IComparable<Person>
    {
        private string _fullNanme;
        private string _id;
        private int _points;
        private double _totalInvoiced;
        private bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        public string FullNanme
        {
            get { return _fullNanme; }
        }
        public abstract decimal GetRating { get; }
        /// <summary>
        /// First constructor.
        /// </summary>
        /// <param name="id">ID to be added</param>
        /// <param name="fullName">fulName to be added</param>
        /// <param name="points">point to be added</param>
        protected Person(string id, string fullName, int points)
        {
            _id = id;
            _fullNanme = fullName;
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

    }
}
