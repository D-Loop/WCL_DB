CREATE DATABASE ProductCompanyDB;
GO
USE ProductCompanyDB;


GO
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY,
    ProductName NVARCHAR(100),
    SupplierID INT,
    CategoryID INT,
    UnitPrice DECIMAL(18, 2),
    UnitsInStock INT
);

GO
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY,
    SupplierName NVARCHAR(100),
    ContactName NVARCHAR(100),
    Phone NVARCHAR(50)
);

GO
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY,
    CustomerName NVARCHAR(100),
    ContactName NVARCHAR(100),
    Phone NVARCHAR(50)
);

GO
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY,
    CustomerID INT,
    EmployeeID INT,
    OrderDate DATETIME,
    ShipAddress NVARCHAR(255),
    ShipCity NVARCHAR(100),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

GO
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    Price DECIMAL(18, 2),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

GO
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY,
    EmployeeName NVARCHAR(100),
    Position NVARCHAR(50),
    HireDate DATETIME
);

GO
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY,
    CategoryName NVARCHAR(100)
);

GO
CREATE TABLE Warehouse (
    WarehouseID INT PRIMARY KEY IDENTITY,
    WarehouseName NVARCHAR(100),
    Location NVARCHAR(100)
);

GO
CREATE TABLE WarehouseStock (
    StockID INT PRIMARY KEY IDENTITY,
    WarehouseID INT,
    ProductID INT,
    Quantity INT,
    FOREIGN KEY (WarehouseID) REFERENCES Warehouse(WarehouseID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

GO
CREATE TABLE SalesReports (
    ReportID INT PRIMARY KEY IDENTITY,
    ReportDate DATETIME,
    TotalSales DECIMAL(18, 2),
    EmployeeID INT,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);
GO
CREATE TABLE SupplyPlan (
    SupplyPlanID INT PRIMARY KEY IDENTITY,
    SupplierID INT,
    WarehouseID INT,
    ProductID INT,
    PlannedQuantity INT,
    SupplyDate DATETIME,
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID),
    FOREIGN KEY (WarehouseID) REFERENCES Warehouse(WarehouseID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

GO
CREATE INDEX idx_ProductName ON Products (ProductName);
CREATE INDEX idx_OrderDate ON Orders (OrderDate);
GO

CREATE TRIGGER trg_UpdateStock
ON OrderDetails
AFTER INSERT
AS
BEGIN
    DECLARE @ProductID INT, @Quantity INT;
    SELECT @ProductID = INSERTED.ProductID, @Quantity = INSERTED.Quantity
    FROM INSERTED;
    
    UPDATE Products
    SET UnitsInStock = UnitsInStock - @Quantity
    WHERE ProductID = @ProductID;
END;

BACKUP DATABASE ProductCompanyDB TO DISK = 'C:\Backup\ProductCompanyDB.bak';

--RESTORE DATABASE ProductCompanyDB FROM DISK = 'C:\Backup\ProductCompanyDB.bak';

GO
CREATE FUNCTION GetProductPrice(@ProductID INT)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @Price DECIMAL(18, 2);
    SELECT @Price = UnitPrice FROM Products WHERE ProductID = @ProductID;
    RETURN @Price;
END;

GO

CREATE FUNCTION GetCustomerName(@CustomerID INT)
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @Name NVARCHAR(100);
    SELECT @Name = CustomerName FROM Customers WHERE CustomerID = @CustomerID;
    RETURN @Name;
END;
GO

CREATE FUNCTION GetTotalStock(@ProductID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Total INT;
    SELECT @Total = SUM(Quantity) FROM WarehouseStock WHERE ProductID = @ProductID;
    RETURN @Total;
END;

GO
CREATE FUNCTION GetCustomerOrderCount(@CustomerID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;
    SELECT @Count = COUNT(*) FROM Orders WHERE CustomerID = @CustomerID;
    RETURN @Count;
END;

GO
CREATE FUNCTION GetTotalSales(@ReportID INT)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @Total DECIMAL(18, 2);
    SELECT @Total = TotalSales FROM SalesReports WHERE ReportID = @ReportID;
    RETURN @Total;
END;

GO

CREATE TRIGGER UpdateOrderTotal
ON OrderDetails
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @OrderID INT;
    SELECT @OrderID = OrderID FROM inserted;
    
    UPDATE Orders
    SET Orders.OrderDate = GETDATE()
    WHERE OrderID = @OrderID;
END;

GO
CREATE TRIGGER TrackStockChanges
ON WarehouseStock
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Ћогика дл€ отслеживани€ изменений
    DECLARE @ProductID INT, @Quantity INT;

    SELECT @ProductID = ProductID, @Quantity = Quantity FROM inserted;
    PRINT 'Product ID: ' + CAST(@ProductID AS NVARCHAR) + ' Quantity: ' + CAST(@Quantity AS NVARCHAR);
END;

GO
CREATE TRIGGER UpdateWarehouseStock
ON OrderDetails
AFTER INSERT
AS
BEGIN
    DECLARE @ProductID INT, @Quantity INT;

    SELECT @ProductID = ProductID, @Quantity = Quantity FROM inserted;

    UPDATE WarehouseStock
    SET Quantity = Quantity - @Quantity
    WHERE ProductID = @ProductID;
END;

GO
CREATE TRIGGER RestrictNegativeStock
ON WarehouseStock
AFTER UPDATE
AS
BEGIN
    IF EXISTS (SELECT * FROM WarehouseStock WHERE Quantity < 0)
    BEGIN
        RAISERROR(' оличество не может быть отрицательным!', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;

GO

CREATE TRIGGER UpdateSalesReport
ON Orders
AFTER INSERT
AS
BEGIN
    DECLARE @Total DECIMAL(18, 2);
    SELECT @Total = SUM(Price * Quantity) FROM OrderDetails WHERE OrderID IN (SELECT OrderID FROM inserted);
    
    INSERT INTO SalesReports (ReportDate, TotalSales)
    VALUES (GETDATE(), @Total);
END;

