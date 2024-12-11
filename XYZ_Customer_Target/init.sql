CREATE DATABASE CatalogDB;
GO

USE CatalogDB;
GO

-- Create tables
CREATE TABLE CatalogType (
    Id INT PRIMARY KEY,
    Type NVARCHAR(100) NOT NULL
);

CREATE TABLE CatalogBrand (
    Id INT PRIMARY KEY,
    Brand NVARCHAR(100) NOT NULL
);

CREATE TABLE CatalogItem (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18, 2) NOT NULL,
    PictureFileName NVARCHAR(100),
    CatalogTypeId INT FOREIGN KEY REFERENCES CatalogType(Id),
    CatalogBrandId INT FOREIGN KEY REFERENCES CatalogBrand(Id),
    AvailableStock INT NOT NULL,
    RestockThreshold INT NOT NULL,
    MaxStockThreshold INT NOT NULL,
    OnReorder BIT NOT NULL
);

-- Seed data
INSERT INTO CatalogType (Id, Type) VALUES
(1, 'Mug'),
(2, 'T-Shirt'),
(3, 'Sheet'),
(4, 'USB Memory Stick');

INSERT INTO CatalogBrand (Id, Brand) VALUES
(1, 'Azure'),
(2, '.NET'),
(3, 'Visual Studio'),
(4, 'SQL Server'),
(5, 'Other');

INSERT INTO CatalogItem (Id, Name, Description, Price, PictureFileName, CatalogTypeId, CatalogBrandId, AvailableStock, RestockThreshold, MaxStockThreshold, OnReorder) VALUES
(1, '.NET Bot Black Hoodie', 'A hoodie for the .NET fan', 19.50, '1.png', 2, 2, 100, 10, 100, 0),
(2, '.NET Black & White Mug', 'A mug for the .NET fan', 8.50, '2.png', 1, 2, 100, 10, 100, 0),
(3, 'Prism White T-Shirt', 'A t-shirt for the Prism fan', 12.00, '3.png', 2, 5, 100, 10, 100, 0),
(4, '.NET Foundation T-Shirt', 'Support the .NET Foundation', 12.00, '4.png', 2, 2, 100, 10, 100, 0);


