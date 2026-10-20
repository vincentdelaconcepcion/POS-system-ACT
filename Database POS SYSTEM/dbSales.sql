CREATE DATABASE Sales;

USE DATABASE Sales;

CREATE TABLE customers(
 customer_id INT PRIMARY KEY,
 first_name VARCHAR(50),
 lat_name VARCHAR(50),
 phone VARCHAR(25),
 email VARCHAR(50),
 street VARCHAR(50),
 city VARCHAR(50),
 [state] VARCHAR(50),
 zip_code VARCHAR(50)
);

CREATE TABLE orders(
order_id INT PRIMARY KEY,
customer_id INT,
order_status VARCHAR(25),
order_date VARCHAR(20),
required_date INT,
shipped_date VARCHAR(20),
store_id INT,
staff_id INT,

FOREIGN KEY (customer_id) REFERENCES customers(customer_id),
FOREIGN KEY (store_id) REFERENCES stores(store_id),
FOREIGN KEY (staff_id) REFERENCES staffs(staff_id)

);

CREATE TABLE staffs(
staff_id INT PRIMARY KEY,
first_name VARCHAR(50),
last_name VARCHAR(50),
email VARCHAR(50),
phone VARCHAR(25),
active VARCHAR(25),
store_id INT,
manager_id INT,

FOREIGN KEY (store_id) REFERENCES stores(store_id)


);

CREATE TABLE stores(
store_id INT PRIMARY KEY,
store_name VARCHAR(50),
phone VARCHAR(25),
email VARCHAR(50),
street VARCHAR(50),
city VARCHAR(50),
[state] VARCHAR(50),
zip_code VARCHAR(50)
);

CREATE TABLE order_items(
order_id INT,
item_id INT PRIMARY KEY,
product_id INT,
quantity INT,
list_price INT,
discount INT,

FOREIGN KEY (product_id) REFERENCES products(product_id),
FOREIGN KEY (order_id) REFERENCES orders(order_id)
);

CREATE TABLE categories(
category_id INT PRIMARY KEY,
category_name VARCHAR(50)
);

EXEC sp_addlinkedserver
	@server = 'ServerB',
	@provider = 