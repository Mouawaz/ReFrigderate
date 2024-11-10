//package com.example.serversideapp;
//
//import Server.UserOuterClass;
//import Server.UserServiceGrpc;
//import io.grpc.stub.StreamObserver;
//
//public class UserServiceImpl extends UserServiceGrpc.UserServiceImplBase {
//    @Override
//    public void getUser(UserOuterClass.UserIdRequest request, StreamObserver<UserOuterClass.User> responseObserver) {
//        String userId = request.getUserid();
//        DBQuery dbQuery = new DBQuery();
//        UserOuterClass.User ans = dbQuery.GetUserById(userId);
//        responseObserver.onNext(ans);
//        responseObserver.onCompleted();
//    }
//}
