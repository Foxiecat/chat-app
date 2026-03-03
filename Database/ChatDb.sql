DROP DATABASE IF EXISTS ChatDb;
CREATE DATABASE ChatDb;
USE ChatDb;

CREATE USER IF NOT EXISTS 'admin'@'localhost' identified by 'admin123';
CREATE USER if not exists 'admin'@'%' identified by 'admin123';

GRANT ALL PRIVILEGES ON ChatDb.* TO 'admin'@'%';
GRANT ALL PRIVILEGES ON ChatDb.* TO 'admin'@'localhost';

FLUSH PRIVILEGES;

CREATE TABLE User(
    Id binary(16) NOT NULL PRIMARY KEY,
    Username varchar(10) NOT NULL,
    Created DATE NOT NULL,
    Updated DATE,
    PasswordHash longtext not null
);
