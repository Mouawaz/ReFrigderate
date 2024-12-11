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
    public User.LoginResponse AddUser(User.CreateUserRequest addRequest) {
        UserLocal attemptedUser = dbUserManager.addUser(addRequest);
        if (attemptedUser == null) {
            return User.LoginResponse.newBuilder().setSuccess(false).build();
        }
        return User.LoginResponse.newBuilder()
                .setSuccess(true)
                .setUserId(attemptedUser.getId())
                .setFullName(attemptedUser.getFirstN() + " " + attemptedUser.getLastN())
                .setPermissions(attemptedUser.getPermissions()).build();
    }

    @Override
    public User.LoginResponse AttemptLogin(User.LoginRequest loginRequest) {
        UserLocal attemptedUser = dbUserManager.getUserByName(loginRequest.getEmail());
        if (attemptedUser != null && attemptedUser.getPassword().equals(loginRequest.getPassword())) {
            return User.LoginResponse.newBuilder()
                    .setSuccess(true)
                    .setUserId(attemptedUser.getId())
                    .setFullName(attemptedUser.getFirstN() + " " + attemptedUser.getLastN())
                    .setPermissions(attemptedUser.getPermissions()).build();
        }
        return User.LoginResponse.newBuilder().setSuccess(false).build();
    }

    @Override
    public User.AllUsersResponse getAllUsers(User.EmptyUser request) {
        User.AllUsersResponse.Builder ans = User.AllUsersResponse.newBuilder();
        for (UserLocal ul : dbUserManager.getAllUsers()){
            ans.addMessages(parseFromLocal(ul));
        }
        return ans.build();
    }

    @Override
    public User.SingleUserResponse getSingleUser(User.UserRequest request) {
        return parseFromLocal(dbUserManager.getSingleUser(request.getUserId()));
    }

    @Override
    public User.UpdateUserResponse updateUser(User.UpdateUserRequest request) {
        return User.UpdateUserResponse.newBuilder()
                .setSuccess(dbUserManager.updateUser(request)).build();
    }
    private User.SingleUserResponse parseFromLocal(UserLocal local){
        return User.SingleUserResponse.newBuilder()
                .setUserid(local.getId())
                .setEmail(local.getEmail())
                .setFullName(local.getFirstN().stripTrailing() + " " + local.getLastN().stripTrailing())
                .setRole(local.getPermissions())
                .build();
    }
}
