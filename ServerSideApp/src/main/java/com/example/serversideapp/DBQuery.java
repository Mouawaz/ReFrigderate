package com.example.serversideapp;

import java.sql.DriverManager;
import java.sql.*;
import java.util.ArrayList;

public class DBQuery {
    public DBQuery() {
    }

    //since we don't have a user yet im just doing with the arraylist
    public ArrayList<String> GetUserByPhone(String phoneNumber) {
        try (Connection connection = getConnected()) {
            PreparedStatement psUser = connection.prepareStatement("SELECT * FROM refrigderate.users WHERE phonenumber = ?");
            psUser.setString(1, phoneNumber);
            ResultSet rsUser = psUser.executeQuery();
            rsUser.next();
            ArrayList<String> user = new ArrayList<>(); //this would be the user
            for (int i = 1; i <= 8; i++) {
                user.add(rsUser.getString(i));
            }
            return user;
//            while (rsUser.next()) {
//            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private Connection getConnected() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres", "postgres", "1234");
    }
}
