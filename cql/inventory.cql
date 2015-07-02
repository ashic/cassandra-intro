CREATE KEYSPACE IF NOT EXISTS inventory
WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 2};

USE inventory;

CREATE TABLE IF NOT EXISTS products ( 
	product_id text,
	sku text,
	title text,
	price_in_pence int,
	categories list<text>,
	stock int,
	PRIMARY KEY (product_id, sku) 
);