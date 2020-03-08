using System;
using System.Collections.Generic;

namespace EventsAssignment
{
    public delegate void TableIsOpenDelegate(object sender, TableOpenEventArgs e);
    public delegate void ChangeMealDelegate(object sender, ChangeMealEventArgs e);

    public class TableOpenEventArgs:EventArgs
    {
        public TableOpenEventArgs()
        {

        }
    }

    public class ChangeMealEventArgs:EventArgs
    {
        public Customer customer;
        public ChangeMealEventArgs(Customer cust)
        {
            this.customer = cust;
        }
    }

    public class Table
    {
        public event TableIsOpenDelegate TableIsOpenEvent;

        public void TableIsOpen()
            {
            Console.WriteLine("Table Is Open!");
                if(TableIsOpenEvent!=null)
                {
                    TableIsOpenEvent(this, new TableOpenEventArgs());
                }
            }
    }
    public enum Meals
    {
        None, Appetizer, Main, Desert, Done
    }

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Meals Meal { get; set; }

        public event ChangeMealDelegate ChangeMealEvent;

        public void ChangeMeal(Meals meal)
        {
            switch(meal)
            {
                case Meals.None:
                    this.Meal = Meals.Appetizer;
                    if(ChangeMealEvent!=null)
                    {
                        ChangeMealEvent(this, new ChangeMealEventArgs(this));
                    }
                    break;
                case Meals.Appetizer:
                    this.Meal = Meals.Main;
                    if(ChangeMealEvent!=null)
                    {
                        ChangeMealEvent(this, new ChangeMealEventArgs(this));
                    }
                    break;
                case Meals.Main:
                    this.Meal = Meals.Desert;
                    if(ChangeMealEvent!=null)
                    {
                        ChangeMealEvent(this, new ChangeMealEventArgs(this));
                    }
                    break;
                case Meals.Desert:
                    this.Meal = Meals.Done;
                    if (ChangeMealEvent != null)
                    {
                        ChangeMealEvent(this, new ChangeMealEventArgs(this));
                    }
                    break;
            }
        }
        
        public void TableIsOpenEventHandler(object sender, TableOpenEventArgs e)
        {
            Console.WriteLine("{0} {1} got a table.", this.FirstName, this.LastName);
        }
    }

    public class Program
    {
        public static void ChangeMealEventHandler(object sender, ChangeMealEventArgs e)
        {
            Console.WriteLine("{0} {1} is having {2}."
                ,e.customer.FirstName,e.customer.LastName,e.customer.Meal);
        }
        public static void Main(string[] args)
        {
            Queue<Customer> cust = new Queue<Customer>();
            Customer cust1 = new Customer();
            Customer cust2 = new Customer();
            Customer cust3 = new Customer();
            Customer cust4 = new Customer();
            Customer cust5 = new Customer();

            cust1.FirstName = "Joe";
            cust1.LastName = "Smith";
            cust1.ChangeMealEvent += ChangeMealEventHandler;
            
            cust2.FirstName = "Jane";
            cust2.LastName = "Jones";
            cust2.ChangeMealEvent += ChangeMealEventHandler;

            cust3.FirstName = "Jack";
            cust3.LastName = "Jump";
            cust3.ChangeMealEvent += ChangeMealEventHandler;

            cust4.FirstName = "Jeff";
            cust4.LastName = "Run";
            cust4.ChangeMealEvent += ChangeMealEventHandler;

            cust5.FirstName = "Jill";
            cust5.LastName = "Hill";
            cust5.ChangeMealEvent += ChangeMealEventHandler;

            cust.Enqueue(cust1);
            cust.Enqueue(cust2);
            cust.Enqueue(cust3);
            cust.Enqueue(cust4);
            cust.Enqueue(cust5);
            
            Table table = new Table();

            foreach(Customer c in cust)
            {
                table.TableIsOpenEvent += c.TableIsOpenEventHandler;
                table.TableIsOpen();
                while(c.Meal!=Meals.Done)
                {
                    c.ChangeMeal(c.Meal);
                }

                table.TableIsOpenEvent -= c.TableIsOpenEventHandler;
            }

            Console.WriteLine("Everyone is full!");
            Console.ReadLine();
        }
    }
}
