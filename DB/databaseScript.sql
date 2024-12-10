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
    role        int DEFAULT 0
);

CREATE TYPE report_type AS ENUM ('Inventory', 'Performance');

CREATE TABLE Report
(
    fridgeID     INT REFERENCES Fridge (fridgeID),
    reportID     SERIAL PRIMARY KEY,
    type         report_type,
    data         TEXT,
    creationDate DATE
);


create TABLE Ingredient
(
    fridgeID     INT REFERENCES Fridge (fridgeID),
    ingredientID SERIAL PRIMARY KEY,
    name         VARCHAR(100),
    cost         DECIMAL(10, 2),
    yellowAmount INT DEFAULT 10,
    redAmount    INT DEFAULT 5,
    yellowDays   INT DEFAULT 7,
    redDays      INT DEFAULT 0
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
    FOREIGN KEY (chefID) REFERENCES "user" (userID)
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

CREATE TYPE meal_course AS ENUM ('Starter', 'Main', 'Dessert');

create TABLE Recipe
(
    fridgeID             INT REFERENCES Fridge (fridgeID),
    recipeID             SERIAL PRIMARY KEY,
    name                 VARCHAR(100),
    instructions         TEXT,
    modificationsAllowed BOOLEAN,
    chefID               INT,
    type                 meal_course,
    FOREIGN KEY (chefID) REFERENCES "user" (userID)
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
    FOREIGN KEY (recipeID) REFERENCES Recipe (recipeID) ON DELETE CASCADE,
    FOREIGN KEY (ingredientID) REFERENCES Ingredient (ingredientID) ON DELETE CASCADE
);


create TABLE MenuRecipe
(
    fridgeID INT REFERENCES Fridge (fridgeID),
    menuID   INT,
    recipeID INT,
    PRIMARY KEY (menuID, recipeID),
    FOREIGN KEY (menuID) REFERENCES Menu (menuID) ON DELETE CASCADE ,
    FOREIGN KEY (recipeID) REFERENCES Recipe (recipeID) ON DELETE CASCADE
);



INSERT INTO Fridge (name, fridgeID)
VALUES ('ReTard', DEFAULT);


INSERT INTO "user" (userID, firstname, lastname, email, password, role)
VALUES (DEFAULT, 'Jhon', 'Doe', 'jdoe@example.com', 'password123', 3),
       (DEFAULT, 'Amantha', 'Smith', 'asmith@example.com', 'password456', 2),
       (DEFAULT, 'Waiter', 'Waiter', 'waiter@example.com', 'iamawaiter', 1),
       (DEFAULT, 'Smough', 'Doe', 'sdoe@example.com', 'password123456', 1);


INSERT INTO Ingredient (ingredientID, name, cost)
VALUES (DEFAULT, 'Tomato', 0.50),
       (DEFAULT, 'Cheese', 2.00);


INSERT INTO Report (reportID, type, data, creationDate)
VALUES (DEFAULT, 'Inventory', 'Monthly stock report', '01-11-2024'),
       (DEFAULT, 'Performance', 'Weekly performance metrics', '08-11-2024');


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
VALUES (DEFAULT, 'Tomato Soup', 'Chop tomatoes and simmer.', TRUE, 1, 'Starter'),
       (DEFAULT, 'Cheese Omelet', 'Whisk eggs and add cheese.', FALSE, 2, 'Starter');


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