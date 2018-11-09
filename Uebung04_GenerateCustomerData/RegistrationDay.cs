using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using static Bogus.DataSets.Name;

namespace Uebung04_GenerateCustomerData
{
    class Customer
    {
        int userId;

        string firstName;

        Gender gender;

        string family;

        string federalState;

        string city;

        DateTime recordStartDate;

        DateTime recordEndDate;

        public Customer(int userId)
        {
            UserId = userId;
        }

        public int UserId { get => userId; set => userId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public Gender Gender { get => gender; set => gender = value; }
        public string Family { get => family; set => family = value; }
        public string FederalState { get => federalState; set => federalState = value; }
        public string City { get => city; set => city = value; }
        public DateTime RecordStartDate { get => recordStartDate; set => recordStartDate = value; }
        public DateTime RecordEndDate { get => recordEndDate; set => recordEndDate = value; }
    }

    class RegistrationDay
    {
        int rows;
        int index;
        DateTime registrationDate;
        Random rnd;
        string path;

        public RegistrationDay(int rows, int index, DateTime registrationDate, Random rnd)
        {
            this.rows = rows;
            this.index = index;
            this.registrationDate = registrationDate;
            this.rnd = rnd;
        }

        public RegistrationDay(int rows, int index, DateTime registrationDate, Random rnd, string path) : this(rows, index, registrationDate, rnd)
        {
            this.path = path;
        }

        public void GenerateCustomerData()
        {
            List<Customer> customers = new List<Customer>();
            registrationDate = new DateTime(registrationDate.Year, registrationDate.Month, registrationDate.Day, 0, 0, 0);
            StreamWriter file;
            if (path != null)
            {
                file = new StreamWriter(Path.Combine(path, "customers_" + registrationDate.ToString("yyyyMMdd") + ".txt"));
            }
            else
            {
                file = new StreamWriter("customers_" + registrationDate.ToString("yyyyMMdd") + ".txt");
            }
            file.Write("CustomerID\tFirstName\tGender\tFamily\tFederalState\tCity\tRecordStartDate\tRecordEndDate");
            for (int i = index * rows; i < (index + 1) * rows; i++)
            {
                var testCustomerss = new Faker<Customer>()
                    //Optional: Call for objects that have complex initialization
                    .CustomInstantiator(f => new Customer(i))
                    //Use an enum outside scope.
                    .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                    //Basic rules using built-in generators
                    .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
                    .RuleFor(u => u.Family, (f, u) => f.Name.LastName(u.Gender))
                    .RuleFor(u => u.Loginname, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                    .RuleFor(u => u.Password, (f, u) => f.Internet.Password())
                    //Optional: After all rules are applied finish with the following action
                    .FinishWith((f, u) =>
                    {
                        Console.WriteLine("Customer Created! Id={0}", u.UserId);
                    });
                
                customers.Add(testCustomers.Generate());
            }
            Console.WriteLine("File: " + (index + 1));
            accounts.Sort((x, y) => DateTime.Compare(x.RegistrationDate, y.RegistrationDate));
            int id = index * rows;
            foreach (Account account in accounts)
            {
                account.Id = id;
                file.Write(account.Id + "\t" + account.Loginname + "\t" + account.Password + "\t" + account.RegistrationDate.ToString("yyyy-MM-dd hh:mm:ss.fff") + "\t" + account.LastLoginDate.ToString(/*"yyyyMMddHHmmss" yyyy-MM-dd hh:mm:ss.fff yyyy-MM-ddTHH:mm:ss*/"yyyy-MM-dd hh:mm:ss.fff") + "\t" +
                    account.CharacterName + "\t" + account.Nation + "\t" + account.Geartype + "\t" + account.Level + "\t" + account.Levelpercentage.ToString("0.##") + "\t" + account.Spi +
                    "\t" + account.Credits + "\t" + account.Fame + "\t" + account.Brigade + "\t" + account.Attack + "\t" + account.Defence + "\t" + account.Evasion + "\t" +
                    account.Fuel + "\t" + account.Spirit + "\t" + account.Shield + "\t" + account.UnusedStatpoints + "\n");
                id++;
            }
            file.Close();
        }
    }
}
