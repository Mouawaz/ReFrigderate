
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


--Dummy data, courtesy of ChatGPT

-- Insert dummy data into "user" table
INSERT INTO "user" (userID, name, email, password, firstname, lastname, dateofbirth, sex, phonenumber) VALUES
(1, 'jdoe', 'jdoe@example.com', 'password123', 'John', 'Doe', '1985-06-15', 'M', '1234567890'),
(2, 'asmith', 'asmith@example.com', 'password456', 'Alice', 'Smith', '1990-11-23', 'F', '0987654321');

-- Insert dummy data into Chef table
INSERT INTO Chef (chefID, position, shiftSchedule) VALUES
(1, 'Head Chef', 'Monday-Friday'),
(2, 'Sous Chef', 'Wednesday-Sunday');

-- Insert dummy data into Waiter table
INSERT INTO Waiter (waiterID, tableAssignment, shiftSchedule) VALUES
(1, 'Table 1-5', 'Tuesday-Saturday'),
(2, 'Table 6-10', 'Friday-Tuesday');

-- Insert dummy data into Owner table
INSERT INTO Owner (ownerID, AccessToReport) VALUES
(1, 'Full'),
(2, 'Limited');

-- Insert dummy data into Ingredient table
INSERT INTO Ingredient (ingredientID, name, cost, quantityInStorage, expirationDate) VALUES
(1, 'Tomato', 0.50, 100, '2024-12-01'),
(2, 'Cheese', 2.00, 50, '2024-11-20');

-- Insert dummy data into Report table
INSERT INTO Report (reportID, type, data, creationDate, ownerID) VALUES
(1, 'Inventory', 'Monthly stock report', '2024-11-01', 1),
(2, 'Performance', 'Weekly performance metrics', '2024-11-08', 2);

-- Insert dummy data into Inventory table
INSERT INTO Inventory (InventoryID, ingredientID, chefID, actionType, quantity, date, expirationDate) VALUES
(1, 1, 1, 'Add', 30, '2024-10-25', '2024-12-01'),
(2, 2, 2, 'Subtract', 10, '2024-11-05', '2024-11-20');

-- Insert dummy data into Alert table
INSERT INTO Alert (alertID, transactionID, thresholdType, status) VALUES
(1, 1, 'Low Stock', 'Pending'),
(2, 2, 'Expiration', 'Resolved');

-- Insert dummy data into Recipe table
INSERT INTO Recipe (recipeID, name, instructions, modificationsAllowed, chefID) VALUES
(1, 'Tomato Soup', 'Chop tomatoes and simmer.', TRUE, 1),
(2, 'Cheese Omelet', 'Whisk eggs and add cheese.', FALSE, 2);

-- Insert dummy data into Menu table
INSERT INTO Menu (menuID, name, status) VALUES
(1, 'Lunch Menu', 'Available'),
(2, 'Dinner Menu', 'Unavailable');

-- Insert dummy data into RecipeIngredient table
INSERT INTO RecipeIngredient (recipeID, ingredientID, quantityNeeded) VALUES
(1, 1, 3),  -- 3 tomatoes needed for Tomato Soup
(2, 2, 1);  -- 1 cheese needed for Cheese Omelet

-- Insert dummy data into MenuRecipe table
INSERT INTO MenuRecipe (menuID, recipeID) VALUES
(1, 1),  -- Tomato Soup on Lunch Menu
(2, 2);  -- Cheese Omelet on Dinner Menu
