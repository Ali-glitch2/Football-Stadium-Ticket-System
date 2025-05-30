using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form1
{
    class  Ticket
    {
        private double price=55.5;
        private int id;
        private string name;
        private int nOfTickets;
        


        public int ID
        {
            get { return id; }
            set {
                if (value >=0)
                {
                    id = value;
                }
                    
                        
                 }
        }


        

        public virtual  double Price
        {
            get
            {
                return price;
            }
           protected set
            {
                if (value >= 0)
                {
                    price= value;
                }
            }
           
        }


        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    name = value;
                }


            }
        }

        public int NOfTickets
        {
            get { return nOfTickets; }
            set
            {
                if (value >= 0)
                {
                    nOfTickets = value;
                }

            }


        }


        public Ticket(int id,string name,int nOfTickets,double price)
        {
            ID = id;
            Name = name;
            NOfTickets = nOfTickets;
            Price = price;
            

        }

       


        public override string ToString()
        {
            return string.Format("Name : {0} , Price without Tax : {1} , Id : {2} , NumberOfTickets : {3}   ", Name, Price.ToString("C"), ID, NOfTicket);
        }


        public virtual double Tax()
        {
            return Price * 1.2; 
        }


    }




    class Vip : Ticket
    {

        public Vip(int id, string name, int nOfTickets, double price)
       : base(id, name, nOfTickets, price)
        {
            Price = 75.7;
        }


        public override double Tax()
        {
            return Price + 0.2 * Price;
        }


        public override string  ToString()
        {
            Console.WriteLine("This is a Vip ticket");
            return base.ToString() + string.Format("Price After Tax : {0}", Tax().ToString("C"));
        }

    }


    class Basic : Ticket
    {
        public  Basic (int id, string name, int nOfTickets, double price)
       : base(id, name, nOfTickets, price)
        {
            
        }

        public override double Tax()
        {
            return Price + 0.2 * Price;
        }



        public override string ToString()
        {
            Console.WriteLine("This is a Basic ticket");
            return base.ToString() + string.Format("Price After Tax : {0}", Tax().ToString("C"));
        }

    }


    class LegendPass : Ticket
    {
        public LegendPass(int id, string name, int nOfTickets, double price)
      : base(id, name, nOfTickets, price)
        {
            Price = 104.6;
        }

        public override double Tax()
        {
            return Price + 0.2 * Price;
        }




        public override string  ToString()
        {

            Console.WriteLine("This is a Legend  ticket");
            return base.ToString() + string.Format("Price After Tax : {0}", Tax().ToString("C"));
        }

    }
    
}
