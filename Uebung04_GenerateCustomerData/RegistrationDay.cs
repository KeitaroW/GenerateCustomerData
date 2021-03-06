﻿using System;
using System.Collections.Generic;
using System.IO;
using Bogus;
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

        public List<Customer> GenerateCustomerData(List<Customer> customers)
        {
            List<Customer> changedData = new List<Customer>();
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
            ChooseRandom random;
            Customer newCustomer;
            foreach (Customer customer in customers)
            {
                random = new ChooseRandom(rnd);
                if (rnd.NextDouble() < 0.001d)
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
                        #if DEBUG
                        .FinishWith((f, u) =>
                        {
                            Console.WriteLine("Customer Created! Id={0}", u.CustomerId);
                        })
                        #endif
                        ;
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
                    newCustomer.Gender = customer.Gender;
                    changedData.Add(newCustomer);
                }
            }
            file.Write("CustomerID\tFirstName\tFamily\tGender\tFederalState\tCity\n");
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
                    #if DEBUG
                    .FinishWith((f, u) =>
                    {
                        Console.WriteLine("Customer Created! Id={0}", u.CustomerId);
                    })
                    #endif
                    ;
                customers.Add(testCustomers.Generate());
            }
            List<Customer> merged = new List<Customer>();
            Console.WriteLine("File: " + (index + 1));
            foreach (Customer customer in customers)
            {
                if (rnd.NextDouble() > 0.001d)
                {
                    if (changedData.Exists(c => c.CustomerId == customer.CustomerId))
                    {
                        Customer changed = changedData.Find(c => c.CustomerId == customer.CustomerId);
                        merged.Add(changed);
                        file.Write(changed.CustomerId + "\t" + changed.FirstName + "\t" + changed.Family + "\t" + changed.Gender + "\t" + changed.FederalState + "\t" + changed.City + "\n");
                    }
                    else
                    {
                        merged.Add(customer);
                        file.Write(customer.CustomerId + "\t" + customer.FirstName + "\t" + customer.Family + "\t" + customer.Gender + "\t" + customer.FederalState + "\t" + customer.City + "\n");
                    }
                }
            }
            file.Close();
            return merged;
        }
    }
}
