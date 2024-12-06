drop schema ReFridgerate cascade;
create schema ReFridgerate;
SET SCHEMA 'refridgerate';

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
DROP TRIGGER IF EXISTS auto_clear_old_inventory ON refridgerate.inventory CASCADE;
SET datestyle TO 'ISO, DMY';


CREATE TABLE Fridge
(
    fridgeID SERIAL PRIMARY KEY,
    name     VARCHAR(100)
);

create TABLE "user"
(
    fridgeID    INT REFERENCES Fridge (fridgeID),
    userID      SERIAL PRIMARY KEY,
    firstname        VARCHAR(100),
    lastname    VARCHAR(100),
    email       VARCHAR(100),
    password    varchar(255) not null,
    role        varchar(20)
);


create TABLE Chef
(
    fridgeID      INT REFERENCES Fridge (fridgeID),
    chefID        INT PRIMARY KEY,
    position      VARCHAR(50),
    shiftSchedule VARCHAR(50),
    FOREIGN KEY (chefID) REFERENCES refridgerate."user" (userID)
);

create TABLE Waiter
(
    fridgeID        INT REFERENCES Fridge (fridgeID),
    waiterID        Int PRIMARY KEY,
    tableAssignment VARCHAR(50),
    shiftSchedule   VARCHAR(50),
    FOREIGN KEY (waiterID) REFERENCES refridgerate."user" (userID)
);

create TABLE Owner
(
    fridgeID       INT REFERENCES Fridge (fridgeID),
    ownerID        INT PRIMARY KEY,
    AccessToReport VARCHAR(20),
    FOREIGN KEY (ownerID) REFERENCES refridgerate."user" (userID)
);

CREATE TYPE report_type AS ENUM ('Inventory', 'Performance');

CREATE TABLE Report
(
    fridgeID     INT REFERENCES Fridge (fridgeID),
    reportID     SERIAL PRIMARY KEY,
    type         report_type,
    data         TEXT,
    creationDate DATE,
    ownerID      INT,
    FOREIGN KEY (ownerID) REFERENCES Owner (ownerID)
);


create TABLE Ingredient
(
    fridgeID     INT REFERENCES Fridge (fridgeID),
    ingredientID SERIAL PRIMARY KEY,
    name         VARCHAR(100),
    cost         DECIMAL(10, 2)
);

CREATE TYPE action_type AS ENUM ('Add', 'Subtract');
CREATE TABLE Inventory
(
    fridgeID         INT REFERENCES Fridge (fridgeID),
    InventoryID      SERIAL PRIMARY KEY,
    ingredientID     INT,
    chefID           INT,
    actionType       action_type,
    quantity         INT,
    date             DATE,
    expirationDate   DATE,
    reasonForRemoval varchar(30),
    FOREIGN KEY (ingredientID) REFERENCES Ingredient (ingredientID),
    FOREIGN KEY (chefID) REFERENCES Chef (chefID)
);

CREATE TYPE threshold_type AS ENUM ('Low Stock', 'Expiration');

CREATE TABLE Alert
(
    fridgeID      INT REFERENCES Fridge (fridgeID),
    alertID       INT PRIMARY KEY,
    transactionID INT,
    thresholdType threshold_type,
    status        VARCHAR(50),
    FOREIGN KEY (transactionID) REFERENCES Inventory (InventoryID)
);

CREATE TABLE Recipe
(
    fridgeID             INT REFERENCES Fridge (fridgeID),
    recipeID             SERIAL PRIMARY KEY,
    name                 VARCHAR(100),
    instructions         TEXT,
    modificationsAllowed BOOLEAN,
    chefID               INT,
    type                 VARCHAR(50),
    FOREIGN KEY (chefID) REFERENCES Chef (chefID)
);


CREATE TYPE menu_status AS ENUM ('Available', 'Unavailable');

CREATE TABLE Menu
(
    fridgeID INT REFERENCES Fridge (fridgeID),
    menuID   SERIAL PRIMARY KEY,
    name     VARCHAR(100),
    status   menu_status
);

create TABLE RecipeIngredient
(
    fridgeID       INT REFERENCES Fridge (fridgeID),
    recipeID       INT,
    ingredientID   INT,
    quantityNeeded INT,
    PRIMARY KEY (recipeID, ingredientID),
    FOREIGN KEY (recipeID) REFERENCES Recipe (recipeID),
    FOREIGN KEY (ingredientID) REFERENCES Ingredient (ingredientID)
);


create TABLE MenuRecipe
(
    fridgeID INT REFERENCES Fridge (fridgeID),
    menuID   INT,
    recipeID INT,
    PRIMARY KEY (menuID, recipeID),
    FOREIGN KEY (menuID) REFERENCES Menu (menuID),
    FOREIGN KEY (recipeID) REFERENCES Recipe (recipeID)
);



INSERT INTO Fridge (name, fridgeID)
VALUES ('ReTard', DEFAULT);


INSERT INTO "user" (userID, firstname, lastname, email, password, role)
VALUES (1, 'Jhon', 'Doe', 'jdoe@example.com', 'password123', 'Commis chef'),
       (2, 'Amantha', 'Smith', 'asmith@example.com', 'password456', 'porter');


INSERT INTO Chef (chefID, position, shiftSchedule)
VALUES (1, 'Head Chef', 'Monday-Friday'),
       (2, 'Sous Chef', 'Wednesday-Sunday');


INSERT INTO Waiter (waiterID, tableAssignment, shiftSchedule)
VALUES (1, 'Table 1-5', 'Tuesday-Saturday'),
       (2, 'Table 6-10', 'Friday-Tuesday');


INSERT INTO Owner (ownerID, AccessToReport)
VALUES (1, 'Full'),
       (2, 'Limited');


INSERT INTO Ingredient (ingredientID, name, cost)
VALUES (DEFAULT, 'Tomato', 0.50),
       (DEFAULT, 'Cheese', 2.00);


INSERT INTO Report (reportID, type, data, creationDate, ownerID)
VALUES (DEFAULT, 'Inventory', 'Monthly stock report', '01-11-2024', 1),
       (DEFAULT, 'Performance', 'Weekly performance metrics', '08-11-2024', 2);


INSERT INTO Inventory (InventoryID, ingredientID, chefID, actionType, quantity, date, expirationDate)
VALUES (DEFAULT, 1, 1, 'Add', 30, '2024-10-25', '2024-12-01'),
       (DEFAULT, 1, 1, 'Add', 40, '2024-10-25', '2024-12-05'),
       (DEFAULT, 2, 2, 'Add', 10, '2024-11-05', '2024-11-10'),
       (DEFAULT, 2, 2, 'Subtract', -10, '2024-11-09', '2024-11-09'),
       (DEFAULT, 2, 2, 'Add', 20, '2024-11-05', '2024-11-25'),
       (DEFAULT, 2, 2, 'Subtract', -10, '2024-11-05', '2024-11-25'),
       (DEFAULT, 2, 2, 'Add', 10, '2024-11-05', '2024-12-01');


INSERT INTO Alert (alertID, transactionID, thresholdType, status)
VALUES (1, 1, 'Low Stock', 'Pending'),
       (2, 2, 'Expiration', 'Resolved');


INSERT INTO Recipe (recipeID, name, instructions, modificationsAllowed, chefID, type)
VALUES
    (DEFAULT, 'Tomato Soup', 'Chop tomatoes and simmer.', TRUE, 1, 'Starter'),
    (DEFAULT, 'Cheese Omelet', 'Whisk eggs and add cheese.', FALSE, 2, 'Starter'),
    (DEFAULT, 'Cheese Omelet', 'Whisk eggs and add cheese.', FALSE, 2, 'Main'),
    (DEFAULT, 'Gateaux marcel', 'bake chocolate mousse at 160 for 40 minutes', FALSE, 1, 'Dessert'),
    (DEFAULT, 'Creme Brulee', 'Emulsify eggs in the cream ', TRUE, 2, 'Dessert');


INSERT INTO Menu (menuID, name, status)
VALUES (DEFAULT, 'Lunch Menu', 'Available'),
       (DEFAULT, 'Dinner Menu', 'Unavailable');


INSERT INTO RecipeIngredient (recipeID, ingredientID, quantityNeeded)
VALUES (1, 1, 3), -- 3 tomatoes needed for Tomato Soup
       (2, 2, 1), -- 1 cheese and 5 tomatoes needed for Cheese Omelet
       (2, 1, 5);

-- Insert dummy data into MenuRecipe table
INSERT INTO MenuRecipe (menuID, recipeID)
VALUES (1, 1), -- Tomato Soup on Lunch Menu
       (2, 2); -- Cheese Omelet on Dinner Menu


CREATE OR REPLACE FUNCTION clear_old_inventory()
    RETURNS void AS
$$
BEGIN

    DELETE
    FROM Inventory
    WHERE date < (CURRENT_DATE - INTERVAL '3 months');


END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION check_and_clear_old_inventory()
    RETURNS TRIGGER AS
$$
BEGIN
    DELETE
    FROM refridgerate.inventory
    WHERE date < (CURRENT_DATE - INTERVAL '3 months');

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
CREATE TRIGGER auto_clear_old_inventory
    AFTER INSERT OR UPDATE
    ON refridgerate.inventory
    FOR EACH STATEMENT
EXECUTE FUNCTION check_and_clear_old_inventory();