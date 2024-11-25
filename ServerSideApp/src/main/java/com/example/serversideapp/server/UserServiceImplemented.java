package com.example.serversideapp.server;

import Server.LoginServiceGrpc;
import Server.User;
import io.grpc.stub.StreamObserver;

public class UserServiceImplemented extends LoginServiceGrpc.LoginServiceImplBase {
    @Override
    public void attemptLogin(User.LoginRequest request, StreamObserver<User.LoginResponse> responseObserver) {
        //request = {username, password}
        super.attemptLogin(request, responseObserver);
    }
}
