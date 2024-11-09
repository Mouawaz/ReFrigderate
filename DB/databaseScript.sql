drop schema ReFrigderate;
create schema ReFrigderate;
SET SCHEMA 'refrigderate';



 create TABLE User (
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
    FOREIGN KEY (chefID) REFERENCES User(userID)
);

  create TABLE Waiter (
    waiterID INT PRIMARY KEY,
    tableAssignment VARCHAR(50),
    shiftSchedule VARCHAR(50),
    FOREIGN KEY (waiterID) REFERENCES User(userID)
);

create TABLE Owner (
    ownerID INT PRIMARY KEY,
    AccessToReport VARCHAR(20),
    FOREIGN KEY (ownerID) REFERENCES User(userID)
);

 create TABLE Report (
    reportID INT PRIMARY KEY,
    type ENUM('Inventory', 'Performance'),
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

 create TABLE Inventory (
    InventoryID INT PRIMARY KEY,
    ingredientID INT,
    chefID INT,
    actionType ENUM('Add', 'Subtract'),
    quantity INT,
    date DATE,
    expirationDate DATE,
    FOREIGN KEY (ingredientID) REFERENCES Ingredient(ingredientID),
    FOREIGN KEY (chefID) REFERENCES Chef(chefID)
);

 create TABLE Alert (
    alertID INT PRIMARY KEY,
    transactionID INT,
    thresholdType ENUM('Low Stock', 'Expiration'),
    status VARCHAR(50),
    FOREIGN KEY (transactionID) REFERENCES InventoryTransaction(transactionID)
);

 create TABLE Recipe (
    recipeID INT PRIMARY KEY,
    name VARCHAR(100),
    instructions TEXT,
    modificationsAllowed BOOLEAN,
    chefID INT,
    FOREIGN KEY (chefID) REFERENCES Chef(chefID)
);

 create TABLE Menu (
    menuID INT PRIMARY KEY,
    name VARCHAR(100),
    status ENUM('Available', 'Unavailable')
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