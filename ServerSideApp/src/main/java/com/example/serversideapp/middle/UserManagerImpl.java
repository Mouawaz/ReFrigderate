package com.example.serversideapp.middle;

import Server.User;
import com.example.serversideapp.back.DBUserManager;
import com.example.serversideapp.shared.UserLocal;

public class UserManagerImpl implements UserManager{
    private DBUserManager dbUserManager;

    public UserManagerImpl(DBUserManager dbUserManager) {
        this.dbUserManager = dbUserManager;
    }

    @Override
    public User.LoginResponse AttemptLogin(User.LoginRequest loginRequest) {
        UserLocal attemptedUser = dbUserManager.getUserByName(loginRequest.getEmail());
        if (attemptedUser == null){
            return User.LoginResponse.newBuilder()
                    .setSuccess(false)
                    .setUserId(-1)
                    .setFullName("N/A")
                    .build();
        }
        if (attemptedUser.getPassword().equals(loginRequest.getPassword())) {
            return User.LoginResponse.newBuilder()
                    .setSuccess(true)
                    .setUserId(attemptedUser.getId())
                    .setFullName(attemptedUser.getFirstN() + " " + attemptedUser.getLastN()).build();
        }
        return User.LoginResponse.newBuilder()
                .setSuccess(false)
                .setUserId(-1)
                .setFullName("N/A")
                .build();
    }
}
