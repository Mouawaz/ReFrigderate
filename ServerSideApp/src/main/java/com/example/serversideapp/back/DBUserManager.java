package com.example.serversideapp.back;

import Server.User;
import com.example.serversideapp.shared.UserLocal;

import java.util.ArrayList;

public interface DBUserManager {
    UserLocal getUserByName(String username);
    UserLocal addUser(User.CreateUserRequest addRequest);
    ArrayList<UserLocal> getAllUsers();
    UserLocal getSingleUser(int id);
    boolean updateUser(User.UpdateUserRequest request);
}
