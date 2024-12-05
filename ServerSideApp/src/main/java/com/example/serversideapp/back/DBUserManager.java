package com.example.serversideapp.back;

import Server.User;
import com.example.serversideapp.shared.UserLocal;

public interface DBUserManager {
    UserLocal getUserByName(String username);
    UserLocal addUser(User.CreateUserRequest addRequest);
}
