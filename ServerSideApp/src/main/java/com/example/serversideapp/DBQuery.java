package com.example.serversideapp;

import Server.UserOuterClass;

import java.sql.DriverManager;
import java.sql.*;
import java.util.ArrayList;

public class DBQuery {
    public DBQuery() {
    }

    //since we don't have a user yet im just doing with the arraylist
    public UserOuterClass.User GetUserById(String id) {
        try (Connection connection = getConnected()) {
            PreparedStatement psUser = connection.prepareStatement("SELECT * FROM refrigderate.users WHERE userid = ?");
            psUser.setString(1, id);
            ResultSet rsUser = psUser.executeQuery();
            rsUser.next();
            UserOuterClass.User user = UserOuterClass.User.newBuilder()
                    .setUserid(rsUser.getString(1))
                    .setEmail(rsUser.getString(2))
                    .setPassword(rsUser.getString(3))
                    .setFirstname(rsUser.getString(4))
                    .setLastname(rsUser.getString(5))
                    .setDateOfBirth(rsUser.getString(6))
                    .setSex(rsUser.getString(7))
                    .setPhoneNumber(rsUser.getString(8)).build();
            return user;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private Connection getConnected() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres", "postgres", "1234");
    }
}
