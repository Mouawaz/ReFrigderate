package com.example.serversideapp.back;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public abstract class DBGeneral {
    protected Connection getConnected() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres", "postgres", "admin");
    }
}
