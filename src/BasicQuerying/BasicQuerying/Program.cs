﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data;
using Cassandra.Data.Linq;

namespace BasicQuerying
{
    class Program
    {
        static void Main(string[] args)
        {
            var cluster = Cluster.Builder()
                .AddContactPoints("192.168.40.3", "192.168.40.4")
                .WithCredentials("cassandra", "cassandra")
                .Build();

            var session = cluster.Connect("inventory");

            var results = session.Execute("select * from products");
            foreach (var result in results)
            {
                Console.WriteLine(result.GetValue<string>("sku"));
            }

            var boundQuery = session.Prepare("select * from products where product_id=?");

            var boundStatement = boundQuery.Bind("p1");

            var results2 = session.Execute(boundStatement);
            foreach (var result in results2)
            {
                Console.WriteLine(result.GetValue<string>("sku"));
            }

            session.Dispose();
            cluster.Dispose();
        }
    }
}
