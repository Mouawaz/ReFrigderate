
DROP TABLE IF EXISTS MenuRecipe CASCADE;
DROP TABLE IF EXISTS RecipeIngredient CASCADE;
DROP TABLE IF EXISTS Menu CASCADE;
DROP TABLE IF EXISTS Recipe CASCADE;
DROP TABLE IF EXISTS Alert CASCADE;
DROP TABLE IF EXISTS Inventory CASCADE;
DROP TABLE IF EXISTS Ingredient CASCADE;
DROP TABLE IF EXISTS Report CASCADE;
DROP TABLE IF EXISTS Owner CASCADE;
DROP TABLE IF EXISTS Waiter CASCADE;
DROP TABLE IF EXISTS Chef CASCADE;
DROP TABLE IF EXISTS "user" CASCADE;
DROP TYPE IF EXISTS menu_status;
DROP TYPE IF EXISTS threshold_type;
DROP TYPE IF EXISTS action_type;
DROP TYPE IF EXISTS report_type;

Drop Function if exists clear_old_inventory() CASCADE;
Drop function if exists trigger_clear_old_inventory() Cascade;
DROP FUNCTION IF EXISTS check_and_clear_old_inventory() CASCADE;
DROP TRIGGER IF EXISTS auto_clear_old_inventory ON refrigderate.inventory CASCADE;
SET datestyle TO 'ISO, DMY';
 create TABLE "user"(
    userID INT PRIMARY KEY,
    name VARCHAR(100),
    email VARCHAR(100),
    password     varchar(255) not null,
    firstname    varchar(255),
    lastname     varchar(255),
    dateofbirth  date,
    sex          char,
    phonenumber  varchar(20)
);


  create TABLE Chef (
    chefID INT PRIMARY KEY,
    position VARCHAR(50),
    shiftSchedule VARCHAR(50),
    FOREIGN KEY (chefID) REFERENCES refrigderate."user"(userID)
);

  create TABLE Waiter (
    waiterID INT PRIMARY KEY,
    tableAssignment VARCHAR(50),
    shiftSchedule VARCHAR(50),
    FOREIGN KEY (waiterID) REFERENCES refrigderate."user"(userID)
);

create TABLE Owner (
    ownerID INT PRIMARY KEY,
    AccessToReport VARCHAR(20),
    FOREIGN KEY (ownerID) REFERENCES refrigderate."user"(userID)
);

CREATE TYPE report_type AS ENUM ('Inventory', 'Performance');

CREATE TABLE Report (
    reportID INT PRIMARY KEY,
    type report_type,
    data TEXT,
    creationDate DATE,
    ownerID INT,
    FOREIGN KEY (ownerID) REFERENCES Owner(ownerID)
);


 create TABLE Ingredient (
    ingredientID INT PRIMARY KEY,
    name VARCHAR(100),
    cost DECIMAL(10, 2),
    quantityInStorage INT,
    expirationDate DATE
);

CREATE TYPE action_type AS ENUM ('Add', 'Subtract');
CREATE TABLE Inventory (
    InventoryID INT PRIMARY KEY,
    ingredientID INT,
    chefID INT,
    actionType action_type,
    quantity INT,
    date DATE,
    expirationDate DATE,
    FOREIGN KEY (ingredientID) REFERENCES Ingredient(ingredientID),
    FOREIGN KEY (chefID) REFERENCES Chef(chefID)
);

CREATE TYPE threshold_type AS ENUM ('Low Stock', 'Expiration');

CREATE TABLE Alert (
    alertID INT PRIMARY KEY,
    transactionID INT,
    thresholdType threshold_type,
    status VARCHAR(50),
    FOREIGN KEY (transactionID) REFERENCES Inventory(InventoryID)
);


 create TABLE Recipe (
    recipeID INT PRIMARY KEY,
    name VARCHAR(100),
    instructions TEXT,
    modificationsAllowed BOOLEAN,
    chefID INT,
    FOREIGN KEY (chefID) REFERENCES Chef(chefID)
);

CREATE TYPE menu_status AS ENUM ('Available', 'Unavailable');

CREATE TABLE Menu (
    menuID INT PRIMARY KEY,
    name VARCHAR(100),
    status menu_status
);

 create TABLE RecipeIngredient (
    recipeID INT,
    ingredientID INT,
    quantityNeeded INT,
    PRIMARY KEY (recipeID, ingredientID),
    FOREIGN KEY (recipeID) REFERENCES Recipe(recipeID),
    FOREIGN KEY (ingredientID) REFERENCES Ingredient(ingredientID)
);


 create TABLE MenuRecipe (
    menuID INT,
    recipeID INT,
    PRIMARY KEY (menuID, recipeID),
    FOREIGN KEY (menuID) REFERENCES Menu(menuID),
    FOREIGN KEY (recipeID) REFERENCES Recipe(recipeID)
);

INSERT INTO "user" (userID, name, email, password, firstname, lastname, dateofbirth, sex, phonenumber) VALUES
(1, 'jdoe', 'jdoe@example.com', 'password123', 'John', 'Doe', '15-06-1985', 'M', '1234567890'),
(2, 'asmith', 'asmith@example.com', 'password456', 'Alice', 'Smith', '23-11-1990', 'F', '0987654321')


INSERT INTO Chef (chefID, position, shiftSchedule) VALUES
(1, 'Head Chef', 'Monday-Friday'),
(2, 'Sous Chef', 'Wednesday-Sunday');


INSERT INTO Waiter (waiterID, tableAssignment, shiftSchedule) VALUES
(1, 'Table 1-5', 'Tuesday-Saturday'),
(2, 'Table 6-10', 'Friday-Tuesday');


INSERT INTO Owner (ownerID, AccessToReport) VALUES
(1, 'Full'),
(2, 'Limited');


INSERT INTO Ingredient (ingredientID, name, cost, quantityInStorage, expirationDate) VALUES
(0, 'Test', 0, 0, '24-12-2024'),
(1, 'Tomato', 0.50, 100, '01-12-2024'),
(2, 'Cheese', 2.00, 50, '20-11-2024');


INSERT INTO Report (reportID, type, data, creationDate, ownerID) VALUES
(1, 'Inventory', 'Monthly stock report', '01-11-2024', 1),
(2, 'Performance', 'Weekly performance metrics', '08-11-2024', 2);


INSERT INTO Inventory (InventoryID, ingredientID, chefID, actionType, quantity, date, expirationDate) VALUES
(1, 1, 1, 'Add', 30, '25-10-2024', '01-12-2024'),
(2, 2, 2, 'Subtract', 10, '05-11-2024', '20-11-2024');


INSERT INTO Alert (alertID, transactionID, thresholdType, status) VALUES
(1, 1, 'Low Stock', 'Pending'),
(2, 2, 'Expiration', 'Resolved');


INSERT INTO Recipe (recipeID, name, instructions, modificationsAllowed, chefID) VALUES
(1, 'Tomato Soup', 'Chop tomatoes and simmer.', TRUE, 1),
(2, 'Cheese Omelet', 'Whisk eggs and add cheese.', FALSE, 2);


INSERT INTO Menu (menuID, name, status) VALUES
(1, 'Lunch Menu', 'Available'),
(2, 'Dinner Menu', 'Unavailable');


INSERT INTO RecipeIngredient (recipeID, ingredientID, quantityNeeded) VALUES
(1, 1, 3),  -- 3 tomatoes needed for Tomato Soup
(2, 2, 1);  -- 1 cheese needed for Cheese Omelet

-- Insert dummy data into MenuRecipe table
INSERT INTO MenuRecipe (menuID, recipeID) VALUES
(1, 1),  -- Tomato Soup on Lunch Menu
(2, 2);  -- Cheese Omelet on Dinner Menu


CREATE OR REPLACE FUNCTION clear_old_inventory()
RETURNS void AS $$
BEGIN

    DELETE FROM Inventory
    WHERE date < (CURRENT_DATE - INTERVAL '3 months');


END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION check_and_clear_old_inventory()
RETURNS TRIGGER AS $$
BEGIN
    DELETE FROM refrigderate.inventory
    WHERE date < (CURRENT_DATE - INTERVAL '3 months');

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
CREATE TRIGGER auto_clear_old_inventory
    AFTER INSERT OR UPDATE ON refrigderate.inventory
    FOR EACH STATEMENT
    EXECUTE FUNCTION check_and_clear_old_inventory();