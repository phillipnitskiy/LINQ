// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		//[Category("Restriction Operators")]
		//[Title("Where - Task 1")]
		//[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
		//public void Linq1()
		//{
		//	int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

		//	var lowNums =
		//		from num in numbers
		//		where num < 5
		//		select num;

		//	Console.WriteLine("Numbers < 5:");
		//	foreach (var x in lowNums)
		//	{
		//		Console.WriteLine(x);
		//	}
		//}

		//[Category("Restriction Operators")]
		//[Title("Where - Task 2")]
		//[Description("This sample return return all presented in market products")]
		//public void Linq2()
		//{
		//	var products =
		//		from p in dataSource.Products
		//		where p.UnitsInStock > 0
		//		select p;

		//	foreach (var p in products)
		//	{
		//		ObjectDumper.Write(p);
		//	}
		//}

        [Category("")]
        [Title("Task 1")]
        [Description("Список всех клиентов, чей суммарный оборот (сумма всех заказов) превосходит некоторую величину X.")]
        public void Linq_Task_1()
        {
            IEnumerable CustomersWithTurnoverMoreThan(decimal X) =>
                dataSource.Customers
                .Select(c => new { c.CustomerID, Turnover = c.Orders.Sum(o => o.Total)})
                .Where(c => c.Turnover > X);          

            foreach (var c in CustomersWithTurnoverMoreThan(10000))
            {
                ObjectDumper.Write(c);
            }

            ObjectDumper.Write("");
            ObjectDumper.Write("");

            foreach (var c in CustomersWithTurnoverMoreThan(100000))
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("")]
        [Title("Task 2")]
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе.")]
        public void Linq_Task_2()
        {
            var customers =
                dataSource.Customers
                .Select(c => new
                {
                    CustomerID = c.CustomerID,
                    Suppliers = dataSource.Suppliers
                                .Where(s => s.Country == c.Country && s.City == c.City)
                                .Select(s => s.SupplierName)
                });

            foreach (var customer in customers)
            {
                ObjectDumper.Write($"{customer.CustomerID} {string.Join(", ", customer.Suppliers)}");
            }

            ObjectDumper.Write("");
            ObjectDumper.Write("");

            var castomersGroup =
                dataSource.Customers
                .GroupJoin(dataSource.Suppliers,
                            c => new { c.Country, c.City },
                            s => new { s.Country, s.City },
                            (c, ss) => new
                            {
                                c.CustomerID,
                                Suppliers = ss.Select(s => s.SupplierName)
                            });

            foreach (var customer in castomersGroup)
            {
                ObjectDumper.Write($"{customer.CustomerID} {string.Join(", ", customer.Suppliers)}");
            }
        }
    }
}
