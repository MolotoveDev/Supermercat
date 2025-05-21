using System;
using System.Drawing;

namespace Supermercat
{
    internal class Program
    {
        public static void MostrarMenu()
        {
            Console.WriteLine("1- UN CLIENT ENTRA AL SUPER I OMPLE EL SEU CARRO DE LA COMPRA");
            Console.WriteLine("2- AFEGIR UN ARTICLE A UN CARRO DE LA COMPRA");
            Console.WriteLine("3- UN CARRO PASSA A CUA DE CAIXA (CHECKIN)");
            Console.WriteLine("4- CHECKOUT DE CUA TRIADA PER L'USUARI");
            Console.WriteLine("5- OBRIR SEGÜENT CUA DISPONIBLE");
            Console.WriteLine("6- INFO CUES");
            Console.WriteLine("7- CLIENTS VOLTANT PEL SUPERMERCAT");
            Console.WriteLine("8- LLISTAR CLIENTS PER RATING (DESCENDENT)");
            Console.WriteLine("9- LLISTAR ARTICLES PER STOCK (DE  - A  +)");
            Console.WriteLine("A- CLOSE QUEUE");
            Console.WriteLine("0- EXIT");
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Supermarket super = new Supermarket("HIPERCAR", "C/Barna 99", "CASHIERS.TXT", "CUSTOMERS.TXT", "GROCERIES.TXT", 2);

            Dictionary<Customer, ShoppingCart> carrosPassejant = new Dictionary<Customer, ShoppingCart>();

            ConsoleKeyInfo tecla;
            do
            {
                Console.Clear();
                MostrarMenu();
                tecla = Console.ReadKey();
                switch (tecla.Key)
                {
                    case ConsoleKey.D1:
                        DoNewShoppingCart(carrosPassejant, super);
                        break;
                    case ConsoleKey.D2:
                        DoAfegirUnArticleAlCarro(carrosPassejant, super);

                        break;
                    case ConsoleKey.D3:
                        DoCheckIn(carrosPassejant, super);

                        break;
                    case ConsoleKey.D4:
                        if (DoCheckOut(super)) Console.WriteLine("BYE BYE. HOPE 2 SEE YOU AGAIN!");
                        else Console.WriteLine("NO S'HA POGUT TANCAR CAP COMPRA");

                        break;
                    case ConsoleKey.D5:
                        DoOpenCua(super);

                        break;
                    case ConsoleKey.D6:
                        DoInfoCues(super);

                        break;

                    case ConsoleKey.D7:
                        DoClientsComprant(carrosPassejant);


                        break;
                    case ConsoleKey.D8:
                        DoListCustomers(super);

                        break;

                    case ConsoleKey.D9:
                        SortedSet<Item> articlesOrdenatsPerEstoc = super.GetItemsByStock();
                        DoListArticlesByStock("LLISTAT D'ARTICLES - DATA " + DateTime.Now, articlesOrdenatsPerEstoc);

                        break;
                    case ConsoleKey.A:
                        DoCloseQueue(super);

                        break;

                    case ConsoleKey.D0:
                        MsgNextScreen("PRESS ANY KEY 2 EXIT");
                        break;

                    default:
                        MsgNextScreen("Error. Prem una tecla per tornar al menú...");
                        break;
                }

            } while (tecla.Key != ConsoleKey.D0);


        }
        //OPCIO 1 - Entra un nou client i se li assigna un carro de la compra. S'omple el carro de la compra
        /// <summary>
        /// Crea un nou carro de la compra assignat a un Customer inactiu
        /// L'omple d'articles aleatòriament 
        /// i l'afegeix als carros que estan passejant pel super
        /// </summary>
        /// <param name="carros">Llista de carros que encara no han entrat a cap 
        /// cua de pagament</param>
        /// <param name="super">necessari per poder seleccionar un client inactiu</param>
        public static void DoNewShoppingCart(Dictionary<Customer, ShoppingCart> carros, Supermarket super)
        {
            Console.Clear();
            Person customer = super.GetAvailableCustomer();
            ShoppingCart cart = new ShoppingCart((Customer)customer, DateTime.Now);
            cart.AddAllRandomly(super.Warehouse);
            carros.Add((Customer)customer, cart);
            MsgNextScreen("PREM UNA TECLA PER ANAR AL MENÚ PRINCIPAL");
        }

        //OPCIO 2 - AFEGIR UN ARTICLE ALEATORI A UN CARRO DE LA COMPRA ALEATORI DELS QUE ESTAN VOLTANT PEL SUPER
        /// <summary>
        /// Dels carros que van passejant pel super, 
        /// es selecciona un carro a l'atzar i un article a l'atzar
        /// i s'afegeix al carro de la compra
        /// amb una quantitat d'unitats determinada
        /// Cal mostrar el carro seleccionat abans i després d'afegir l'article.
        /// </summary>
        /// <param name = "carros" > Llista de carros que encara no han entrat a cap 
        /// cua de pagament</param>
        /// <param name = "super" > necessari per poder seleccionar un article del magatzem</param>
        public static void DoAfegirUnArticleAlCarro(Dictionary<Customer, ShoppingCart> carros, Supermarket super)
        {
            Console.Clear();
            Random random = new Random();
            int numeroProducte = random.Next(0, super.Warehouse.Count - 1);
            double valor;
            ShoppingCart cart = carros.ElementAt(random.Next(0, carros.Count - 1)).Value;
            Item producte = super.Warehouse.ElementAt(numeroProducte).Value;
            Console.WriteLine($"carros seleccionat: {cart}");
            Console.WriteLine($"producte seleccionat {producte}");
            if(producte.PackagingType == "Kg") { valor = random.NextDouble() * 2; }
            else { valor = random.Next(0, 5); }
            cart.AddOne(producte, valor);
            Console.WriteLine($"carros seleccionat despres: {cart}");
            MsgNextScreen("PREM UNA TECLA PER ANAR AL MENÚ PRINCIPAL");
        }
        // OPCIO 3 : Un dels carros que van pululant pel super  s'encua a una cua activa
        // La selecció del carro i de la cua és aleatòria
        /// <summary>
        /// Agafem un dels carros passejant (random) i l'encuem a una de les cues actives
        /// de pagament.
        /// També hem d'eliminar el carro seleccionat de la llista de carros que passejen 
        /// Si no hi ha cap carro passejant o no hi ha cap linia activa, cal donar missatge 
        /// </summary>
        /// <param name="carros">Llista de carros que encara no han entrat a cap 
        /// cua de pagament</param>
        /// <param name="super">necessari per poder encuar un carro a una linia de caixa</param>
        public static void DoCheckIn(Dictionary<Customer, ShoppingCart> carros, Supermarket super)
        {
            Console.Clear();
            if(carros.Count > 0 && super.ActiveLines > 0)
            {
                bool trobat = false;
                Random random = new Random();
                CheckOutLine line = super.GetCheckOutLine(random.Next(1, super.ActiveLines + 1));
                int numeroCarro = random.Next(0, carros.Count - 1);
                line.CheckIn(carros.ElementAt(numeroCarro).Value);
                carros.Remove(carros.ElementAt(numeroCarro).Key);
            }
            MsgNextScreen("PREM UNA TECLA PER ANAR AL MENÚ PRINCIPAL");
        }

        // OPCIO 4 - CHECK OUT D'UNA CUA TRIADA PER L'USUARI
        /// <summary>
        /// Es demana per teclat una cua de les actives (1..ActiveLines) 
        /// i es fa el checkout del ShoppingCart que toqui
        /// Si no hi ha cap carro a la cua triada, es dona un missatge
        /// </summary>
        /// <param name="super">necessari per fer el checkout</param>
        /// <returns>true si s'ha pogut fer el checkout. False en cas contrari</returns>

        public static bool DoCheckOut(Supermarket super)
        {
            bool fet = true;
            int line;
            Console.Clear();
            Console.WriteLine("DONEM LA LINEA: ");
            line = Convert.ToInt32(Console.ReadLine());
            if(!super.GetCheckOutLine(line).CheckOut()) { fet = false; }; /// si no es pot es pasa fet a false.
            MsgNextScreen("PREM UNA TECLA PER ANAR AL MENÚ PRINCIPAL");
            return fet;
        }
        /// <summary>
        /// En cas que hi hagin cues disponibles per obrir, s'obre la 
        /// següent linia disponible
        /// </summary>
        /// <param name="super"></param>
        /// <returns>true si s'ha pogut obrir la cua</returns>
        // OPCIO 5 : Obrir la següent cua disponible (si n'hi ha)
        public static bool DoOpenCua(Supermarket super)
        {
            Console.Clear();
            bool fet = true;
            int lineasAnteriores = super.ActiveLines;
            super.OpenCheckOutLine(super.ActiveLines+1);
            if (lineasAnteriores == super.ActiveLines) {  fet = false; }; // si las linias activas siguen siendo de la misma cantidad (osea no se agrego otra) se deja fet com a false.
            MsgNextScreen("PREM UNA TECLA PER ANAR AL MENÚ PRINCIPAL");
            return fet;
        }

        //OPCIO 6 : Llistar les cues actives
        /// <summary>
        /// Es llisten totes les cues actives des de la 1 fins a ActiveLines.
        /// Apretar una tecla després de cada cua activa
        /// </summary>
        /// <param name="super"></param>
        public static void DoInfoCues(Supermarket super)
        {
            Console.Clear();
            for(int i = 1; i <= super.ActiveLines; i++)
            {
                Console.WriteLine(super.GetCheckOutLine(i).ToString());
            }
            MsgNextScreen("PREM UNA TECLA PER CONTINUAR");
        }


        // OPCIO 7 - Mostrem tots els carros de la compra que estan voltant
        // pel super però encara no han anat a cap cua per pagar
        /// <summary>
        /// Es mostren tots els carros que no estan en cap cua.
        /// Cal apretar una tecla després de cada carro
        /// </summary>
        /// <param name="carros"></param>
        public static void DoClientsComprant(Dictionary<Customer, ShoppingCart> carros)
        {
            Console.Clear();
            foreach (ShoppingCart cart in carros.Values)
            {
                Console.WriteLine(cart.ToString());
            }
            Console.WriteLine($"CARROS VOLTANT PEL SUPER (PENDENTS D'ANAR A PAGAR): {carros.Count}");
            MsgNextScreen("PREM UNA TECLA PER CONTINUAR");
        }

        //OPCIO 8 : LListat de clients per rating
        /// <summary>
        /// Cal llistar tots els clients de més a menys rating
        /// Per poder veure bé el llistat, primer heu de fer uns quants
        /// checkouts i un cop fets, fer el llistat. Aleshores els
        /// clients que han comprat tindran ratings diferents de 0
        /// Jo he fet una sentencia linq per solucionar aquest apartat
        /// </summary>
        /// <param name="super"></param>
        public static void DoListCustomers(Supermarket super)
        {
            Console.Clear();

            int i = 0;
            Dictionary<string, Person> diccionari = super.Customers;
            Customer[] array = new Customer[diccionari.Count];

            foreach (KeyValuePair<string, Person> costumer in diccionari)
            {
                array[i++] = (Customer)costumer.Value;
            }

            Array.Sort(array);

            foreach (Customer customer in array)
            {
                Console.WriteLine(customer.ToString());
            }

            MsgNextScreen("PREM UNA TECLA PER CONTINUAR");
        }

        // OPCIO 9
        /// <summary>
        /// Llistar de menys a més estoc, tots els articles del magatzem
        /// </summary>
        /// <param name="header">Text de capçalera del llistat</param>
        /// <param name="items">articles que ja vindran preparats en la ordenació desitjada</param>
        public static void DoListArticlesByStock(String header, SortedSet<Item> items)
        {
            Console.Clear();
            Console.WriteLine(header);
            foreach (Item i in items)
            {
                Console.WriteLine(i.ToString());
            }

            MsgNextScreen("PREM UNA TECLA PER CONTINUAR");
        }

        // OPCIO A : Tancar cua. Només si no hi ha cap client
        /// <summary>
        /// Començant per la última cua disponible, tanca la primera que trobi sense
        /// cap carro encuat. (primer mirem la número "ActiveLines" després ActiveLines-1 ...
        /// Fins trobar una que estigui buida. La que trobem la eliminarem
        /// Cal afegir la propietat Empty a la classe ChecOutLine i  a la classe Supermarket:
        /// el mètode public static bool RemoveQueue(Supermarket super, int lineToRemove)
        /// que elimina la cua amb número = lineToRemove i retorna true en cas que l'hagi 
        /// pogut eliminar (perquè no hi ha cap carro pendent de pagament)
        /// </summary>
        /// <param name="super"></param>
        public static void DoCloseQueue(Supermarket super)
        {
            Console.Clear();
            int line2Remove = 0;
            bool found = false;
            int i = super.ActiveLines;

            while (!found && i != 0)
            {
                if (super.GetCheckOutLine(i).Empty)
                {
                    line2Remove = i;
                    found = true;
                }
                i--;
            }

            if (line2Remove == 0) Console.WriteLine("No hi ha cues disponibles per tancar");
            else
            {
                if (Supermarket.RemoveQueue(super, line2Remove)) Console.WriteLine("Cua tancada correctament");
                else Console.WriteLine("No s'ha pogut tancar la cua");
            }

            MsgNextScreen("PREM UNA TECLA PER CONTINUAR");
        }


        public static void MsgNextScreen(string msg)
        {
            Console.WriteLine(msg);
            Console.ReadKey();
        }

    }
}
