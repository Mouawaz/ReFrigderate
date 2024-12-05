package com.example.serversideapp.middle;

import Server.User;

public interface UserManager {
    User.LoginResponse AttemptLogin(User.LoginRequest loginRequest);
    User.LoginResponse AddUser(User.CreateUserRequest addRequest);
}
