package com.example.serversideapp.middle;

import Server.User;
import com.example.serversideapp.back.DBUserQuery;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class UserTest {
    private UserManager um;

    @BeforeEach
    void setUp() {
        um = new UserManagerImpl(new DBUserQuery());
    }

    @Test
    void attemptLogin() {
        User.LoginRequest req = User.LoginRequest.newBuilder()
                .setEmail("jdoe@example.com")
                .setPassword("password123").build();
        User.LoginResponse resp = um.AttemptLogin(req);
        assertTrue(resp.getSuccess());
        assertEquals(resp.getUserId(), 1);
    }

    @Test
    void addUser() {
    }

    @Test
    void getAllUsers() {
        User.SingleUserResponse tried = User.SingleUserResponse.newBuilder()
                .setFullName("Jhon Doe")
                .setRole(3)
                .setEmail("jdoe@example.com")
                .setUserid(1).build();
        User.AllUsersResponse ans = um.getAllUsers(User.EmptyUser.newBuilder().build());
        assertEquals(ans.getMessages(0), tried);
    }

    @Test
    void getSingleUser() {
        User.SingleUserResponse tried = User.SingleUserResponse.newBuilder()
                .setFullName("Jhon Doe")
                .setRole(3)
                .setEmail("jdoe@example.com")
                .setUserid(1).build();
        User.SingleUserResponse ans = um.getSingleUser(User.UserRequest.newBuilder()
                .setUserId(1).build());
        assertEquals(ans, tried);
    }

    @Test
    void updateUser() {
        User.SingleUserResponse supposed = User.SingleUserResponse.newBuilder()
                .setFullName("Jhon Doe")
                .setRole(1)
                .setEmail("jdoe@example.com")
                .setUserid(1).build();

        um.updateUser(User.UpdateUserRequest.newBuilder()
                .setUserId(1)
                .setRole(1).build());
        User.SingleUserResponse actual = um.getSingleUser(User.UserRequest.newBuilder().setUserId(1).build());
        assertEquals(actual, supposed);
        um.updateUser(User.UpdateUserRequest.newBuilder()
                .setUserId(1)
                .setRole(3).build());
    }
}