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
        int customerId;

        string firstName;

        Gender gender;

        string family;

        string federalState;

        string city;

        DateTime recordStartDate;

        DateTime recordEndDate;

        public Customer(int customerId)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get => customerId; set => customerId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public Gender Gender { get => gender; set => gender = value; }
        public string Family { get => family; set => family = value; }
        public string FederalState { get => federalState; set => federalState = value; }
        public string City { get => city; set => city = value; }
        public DateTime RecordStartDate { get => recordStartDate; set => recordStartDate = value; }
        public DateTime RecordEndDate { get => recordEndDate; set => recordEndDate = value; }
    }

    class ChooseRandom
    {
        bool family;
        bool federalState;
        bool city;

        public ChooseRandom(Random rnd)
        {
            while (!family && !federalState && !city)
            {
                if (rnd.NextDouble() < 1.0d / 3.0d)
                {
                    Family = true;
                }
                if (rnd.NextDouble() < 1.0d / 3.0d)
                {
                    FederalState = true;
                }
                if (rnd.NextDouble() < 1.0d / 3.0d)
                {
                    City = true;
                }
            }
        }

        public bool Family { get => family; set => family = value; }
        public bool FederalState { get => federalState; set => federalState = value; }
        public bool City { get => city; set => city = value; }
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

        public List<Customer> GenerateCustomerData(List<Customer> changeData)
        {
            List<Customer> customers = new List<Customer>();
            List<Customer> changedChangeData = new List<Customer>(changeData);
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
            file.Write("CustomerID\tFirstName\tFamily\tGender\tFederalState\tCity");
            for (int i = index * rows; i < (index + 1) * rows; i++)
            {
                var testCustomers = new Faker<Customer>()
                    //Optional: Call for objects that have complex initialization
                    .CustomInstantiator(f => new Customer(i))
                    //Use an enum outside scope.
                    .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                    //Basic rules using built-in generators
                    .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
                    .RuleFor(u => u.Family, (f, u) => f.Name.LastName(u.Gender))
                    .RuleFor(u => u.FederalState, (f, u) => f.Address.State())
                    .RuleFor(u => u.City, (f, u) => f.Address.City())
                    //Optional: After all rules are applied finish with the following action
                    .FinishWith((f, u) =>
                    {
                        Console.WriteLine("Customer Created! Id={0}", u.CustomerId);
                    });
                customers.Add(testCustomers.Generate());
            }
            Console.WriteLine("File: " + (index + 1));
            Customer newCustomer;
            ChooseRandom random;
            foreach (Customer customer in changeData)
            {
                random = new ChooseRandom(rnd);
                if (rnd.NextDouble() < 0.1d)
                {
                    var testCustomers = new Faker<Customer>()
                    //Optional: Call for objects that have complex initialization
                        .CustomInstantiator(f => new Customer(customer.CustomerId))
                        //Use an enum outside scope.
                        .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                        //Basic rules using built-in generators
                        .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
                        .RuleFor(u => u.Family, (f, u) => f.Name.LastName(u.Gender))
                        .RuleFor(u => u.FederalState, (f, u) => f.Address.State())
                        .RuleFor(u => u.City, (f, u) => f.Address.City())
                        //Optional: After all rules are applied finish with the following action
                        .FinishWith((f, u) =>
                        {
                            Console.WriteLine("Customer Created! Id={0}", u.CustomerId);
                        });
                    newCustomer = testCustomers.Generate();
                    newCustomer.FirstName = customer.FirstName;
                    if (!random.Family)
                        newCustomer.Family = customer.Family;
                    if (!random.FederalState)
                    {
                        newCustomer.FederalState = customer.FederalState;
                        if (!random.City)
                            newCustomer.City = customer.City;
                    }
                    changedChangeData.Remove(customer);
                    file.Write(newCustomer.CustomerId + "\t" + newCustomer.FirstName + "\t" + newCustomer.Family + "\t" + newCustomer.Gender + "\t" + newCustomer.FederalState + "\t" + newCustomer.City + "\n");
                    if (rnd.NextDouble() < 0.1d)
                    {
                        changedChangeData.Add(newCustomer);
                    }
                }
            }
            foreach (Customer customer in customers)
            {
                if (rnd.NextDouble() < 0.1d)
                {
                    changedChangeData.Add(customer);
                }
                file.Write(customer.CustomerId + "\t" + customer.FirstName + "\t" + customer.Family + "\t" + customer.Gender + "\t" + customer.FederalState + "\t" + customer.City + "\n");
            }
            file.Close();
            return changedChangeData;
        }
    }
}
