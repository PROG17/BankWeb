﻿using BankWeb.Configs;
using BankWeb.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BankWeb.Repositories
{
    public interface IBankRepository
    {
        IDictionary<int, Account> GetAllAccounts();
        IDictionary<int, Customer> GetAllCustomers();
    }

    public class BankRepository : IBankRepository
    {
        private readonly IList<Customer> _customers;
        private readonly IList<Account> _accounts;

        private readonly IOptions<ApplicationSettingsConfig> _config;

        // For testing
        public BankRepository() { }

        public BankRepository(IOptions<ApplicationSettingsConfig> config)
        {
            _config = config;

            if (_customers == null)
            {
                _customers = new List<Customer>();
                _accounts = new List<Account>();

                LoadData();
            }
        }

        public IDictionary<int, Account> GetAllAccounts()
        {
            return _accounts.ToDictionary(x => x.AccountId, x => x);
        }

        public IDictionary<int, Customer> GetAllCustomers()
        {
            return _customers.ToDictionary(x => x.CustomerId, x => x);
        }


        private void LoadData()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _config.Value.Filename);

            using (var sr = new StreamReader(path))
            {
                int numberOfCustomers = int.Parse(sr.ReadLine());

                var props = typeof(Customer).GetProperties().Where(x => x.GetCustomAttribute<IgnoreAttribute>() == null).ToArray();

                for (int i = 0; i < numberOfCustomers; i++)
                {
                    var row = sr.ReadLine().Split(';');
                    _customers.Add(NewObject<Customer>(props, row));
                }

                int numberOfAccounts = int.Parse(sr.ReadLine());
                props = typeof(Account).GetProperties();

                for (int i = 0; i < numberOfAccounts; i++)
                {
                    var row = sr.ReadLine().Split(';');
                    _accounts.Add(NewObject<Account>(props, row));
                }
            }

            foreach (var customer in _customers)
                customer.Accounts = _accounts.Where(x => x.CustomerId == customer.CustomerId).ToArray();
        }

        private static T NewObject<T>(PropertyInfo[] props, string[] data) where T : new()
        {
            var result = new T();

            for (int i = 0; i < props.Length; i++)
                ParseData(result, props[i], data[i]);

            return result;
        }

        private static void ParseData(object obj, PropertyInfo prop, string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            if (prop.PropertyType == typeof(string))
                prop.SetValue(obj, data);
            else if (prop.PropertyType == typeof(int))
                prop.SetValue(obj, int.Parse(data));
            else if (prop.PropertyType == typeof(decimal))
                prop.SetValue(obj, decimal.Parse(data, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
        }
    }
}
