package com.example.serversideapp.back;

import Server.User;
import com.example.serversideapp.shared.UserLocal;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

public class DBUserQuery extends DBGeneral implements DBUserManager{
    @Override
    public UserLocal getUserByName(String email) {
        try (Connection connection = getConnected()){
            PreparedStatement psUser = connection.prepareStatement("SELECT userid, email, password, firstname, lastname, role  FROM refridgerate.user WHERE email = ? ORDER BY userid");
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
                    rsUser.getString(5),
                    rsUser.getInt(6));
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

    @Override
    public ArrayList<UserLocal> getAllUsers() {
        try(Connection connection = getConnected()){
            ArrayList<UserLocal> ans = new ArrayList<>();
            PreparedStatement psGetAllUsers = connection.prepareCall("SELECT userid, email, firstname, lastname, role FROM refridgerate.user ORDER BY userid");
            ResultSet rsGetAllUsers = psGetAllUsers.executeQuery();
            while (rsGetAllUsers.next()){
                ans.add(new UserLocal(
                        rsGetAllUsers.getInt(1),
                        rsGetAllUsers.getString(2),
                        "",
                        rsGetAllUsers.getString(3),
                        rsGetAllUsers.getString(4),
                        rsGetAllUsers.getInt(5)
                ));
            }
            return ans;
        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }

    @Override
    public UserLocal getSingleUser(int id) {
        for (UserLocal ul : getAllUsers()){
            if (ul.getId() ==id){
                return ul;
            }
        }
        throw new RuntimeException("No user with id " + id + " found");
    }

    @Override
    public boolean updateUser(User.UpdateUserRequest request) {
        try(Connection connection = getConnected()){
            PreparedStatement psUpdateUser = connection.prepareStatement("UPDATE refridgerate.user SET role = ? WHERE userid = ?");
            psUpdateUser.setInt(1, request.getRole());
            psUpdateUser.setInt(2, request.getUserId());
            return psUpdateUser.executeUpdate() > 0;
        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }
}
