using System;
using System.Collections.Generic;
using Cassandra;
using Cassandra.Mapping;
using Cassandra.Mapping.Attributes;

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

            var mapping = new Mapper(session);

            var products = mapping.Fetch<Product>("select * from products where product_id=? and sku=?", "p1", "psku1");
            
            foreach (var product in products)
            {
                Console.WriteLine("SKU: {0}, Price: {1}", product.Sku, product.Price);
            }

            session.Dispose();
            cluster.Dispose();
        }
    }

    public class Product
    {
        [PartitionKey]
        [Column("product_id")]
        public string ProductId { get; set; }
        [ClusteringKey(0)]
        [Column("sku")]
        public string Sku { get; set; }
        [Column("price_in_pence")]
        public int  Price { get; set; }
        [Column("categories")]
        public List<string> Categories { get; set; }
    }
}
