CREATE DATABASE Production;

USE DATABASE Production;

CREATE TABLE products(
product_id INT PRIMARY KEY,
product_name VARCHAR(50),
brand_id INT,
category_id INT,
model_year VARCHAR(25),
list_price INT,

FOREIGN KEY(category_id) REFERENCES categories(category_id),
FOREIGN KEY(brand_id) REFERENCES brand(brand_id)
);

CREATE TABLE stocks(
store_id INT,
product_id INT,
quantity VARCHAR(25),

FOREIGN KEY (store_id) REFERENCES stores(store_id),
FOREIGN KEY (product_id) REFERENCES products(product_id)
);

CREATE TABLE brand(
brand_id INT PRIMARY KEY,
brand_name VARCHAR(50)
);