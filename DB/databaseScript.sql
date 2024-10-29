drop schema ReFrigderate;
create schema ReFrigderate;
SET SCHEMA 'refrigderate';
drop table users;
create table users(
    userid       varchar(255)    not null
        primary key,
    email        varchar(255) not null
        unique,
    password     varchar(255) not null,
    firstname    varchar(255),
    lastname     varchar(255),
    dateofbirth  date,
    sex          char,
    phonenumber  varchar(20)
);

 INSERT INTO users (userid, email, password, firstname, lastname, dateofbirth, sex, phonenumber)
VALUES
    ('realSigma', 'user2@example.com', 'password2', 'Alice', 'Smith', '1985-05-15', 'F', '9876543210'),
    ('BetaBehavior', 'user3@example.com', 'password3', 'Bob', 'Johnson', '1978-09-30', 'M', '5551234567'),
    ('kitchenWench', 'user4@example.com', 'password4', 'Emily', 'Brown', '2000-03-20', 'F', '7779876543'),
    ('HeeHeee', 'user5@example.com', 'password5', 'Michael', 'Davis', '1995-11-10', 'M', '1112223333');
