package com.example.serversideapp.server;

import Server.UserServiceGrpc;
import Server.User;
import com.example.serversideapp.middle.UserManager;
import io.grpc.stub.StreamObserver;

public class UserServiceImplemented extends UserServiceGrpc.UserServiceImplBase {
    private UserManager userManager;

    public UserServiceImplemented(UserManager userManager) {
        this.userManager = userManager;
    }

    @Override
    public void attemptLogin(User.LoginRequest request, StreamObserver<User.LoginResponse> responseObserver) {
        //request = {username, password}
        User.LoginResponse resp = userManager.AttemptLogin(request);
        responseObserver.onNext(resp);
        if (resp.getSuccess()) {
            responseObserver.onCompleted();
        } else {
            responseObserver.onError(new Throwable("Something went wrong :("));
        }
    }

    @Override
    public void addUser(User.CreateUserRequest request, StreamObserver<User.LoginResponse> responseObserver) {

        User.LoginResponse resp = userManager.AddUser(request);
        responseObserver.onNext(resp);
        if (resp.getSuccess()) {
            responseObserver.onCompleted();
        } else {
            responseObserver.onError(new Throwable("Something went wrong :("));
        }
    }

    @Override
    public void getAllUsers(User.EmptyUser request, StreamObserver<User.AllUsersResponse> responseObserver) {
        responseObserver.onNext(userManager.getAllUsers(request));
        responseObserver.onCompleted();
    }

    @Override
    public void getSingleUser(User.UserRequest request, StreamObserver<User.SingleUserResponse> responseObserver) {
        responseObserver.onNext(userManager.getSingleUser(request));
        responseObserver.onCompleted();
    }

    @Override
    public void updateUser(User.UpdateUserRequest request, StreamObserver<User.UpdateUserResponse> responseObserver) {
        responseObserver.onNext(userManager.updateUser(request));
        responseObserver.onCompleted();
    }
}

