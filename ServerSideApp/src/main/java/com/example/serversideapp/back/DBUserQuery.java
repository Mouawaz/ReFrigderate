package com.example.serversideapp.back;

import Server.User;
import com.example.serversideapp.shared.UserLocal;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class DBUserQuery extends DBGeneral implements DBUserManager{
    @Override
    public UserLocal getUserByName(String email) {
        try (Connection connection = getConnected()){
            PreparedStatement psUser = connection.prepareStatement("SELECT userid, email, password, firstname, lastname  FROM refridgerate.user WHERE email = ?");
            psUser.setString(1, email);
            ResultSet rsUser = psUser.executeQuery();
            if (!rsUser.next()){
                return null;
            }
            return new UserLocal(rsUser.getInt(1),
                    rsUser.getString(2),
                    rsUser.getString(3),
                    rsUser.getString(4),
                    rsUser.getString(5));
        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }

    @Override
    public UserLocal addUser(User.CreateUserRequest addRequest) {
        try(Connection connection = getConnected()) {
            PreparedStatement psAddUser = connection.prepareStatement("INSERT INTO refridgerate.user VALUES (1, DEFAULT, ?, ?, ?, ?, 'Role placeholder')");
            psAddUser.setString(1, addRequest.getFirstname());
            psAddUser.setString(2, addRequest.getLastname());
            psAddUser.setString(3, addRequest.getEmail());
            psAddUser.setString(4, addRequest.getPassword());
            psAddUser.executeUpdate();
            return getUserByName(addRequest.getEmail());
        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }
}
