package com.example.serversideapp.back;

import com.example.serversideapp.shared.UserLocal;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;

public class DBUserQuery extends DBGeneral implements DBUserManager{
    @Override
    public UserLocal getUserByName(String username) {
        try (Connection connection = getConnected()){
            PreparedStatement psUser = connection.prepareStatement("SELECT userid, name, password, firstname, lastname FROM refridgerate.user WHERE name = ?;");
            psUser.setString(1, username);


        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }
}
