using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uebung04_GenerateCustomerData
{
    class GenerateCustomerData
    {
        public static void Main(string[] args)
        {
            if (args.Length == 5)
            {
                Random rnd = new Random();
                int year = Int32.Parse(args[0]);
                int month = Int32.Parse(args[1]);
                int day = Int32.Parse(args[2]);
                int numOfDays = Int32.Parse(args[3]);
                int rowsPerDay = Int32.Parse(args[4]);
                DateTime dateTime = new DateTime(year, month, day);
                List<Customer> changeData = new List<Customer>();
                RegistrationDay registrationDay;
                for (int i = 0; i < numOfDays; i++)
                {
                    registrationDay = new RegistrationDay(rowsPerDay, i, dateTime, rnd);
                    changeData = registrationDay.GenerateCustomerData(changeData);
                    dateTime = dateTime.AddDays(1);
                }
                Console.Read();
            }
            else if (args.Length == 6)
            {
                Random rnd = new Random();
                int year = Int32.Parse(args[1]);
                int month = Int32.Parse(args[2]);
                int day = Int32.Parse(args[3]);
                int numOfDays = Int32.Parse(args[4]);
                int rowsPerDay = Int32.Parse(args[5]);
                DateTime dateTime = new DateTime(year, month, day);
                List<Customer> customers = new List<Customer>();
                RegistrationDay registrationDay;
                for (int i = 0; i < numOfDays; i++)
                {
                    registrationDay = new RegistrationDay(rowsPerDay, i, dateTime, rnd, args[0]);
                    customers = registrationDay.GenerateCustomerData(customers);
                    dateTime = dateTime.AddDays(1);
                }
                Console.Read();
            }
            else
            {
                Console.WriteLine("Usage: Uebung04_GenerateCustomerData.exe <year> <month> <day> <number of days> <rows per day>");
                Console.WriteLine("or");
                Console.WriteLine("Usage: Uebung04_GenerateCustomerData.exe \"<path>\" <year> <month> <day> <number of days> <rows per day>");
            }
        }
    }
}
