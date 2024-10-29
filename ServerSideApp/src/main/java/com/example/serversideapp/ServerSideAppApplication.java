package com.example.serversideapp;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.sql.SQLException;

@SpringBootApplication
public class ServerSideAppApplication {

    public static void main(String[] args) {
        SpringApplication.run(ServerSideAppApplication.class, args);
//        DBQuery dbQuery = new DBQuery();
//        System.out.println(dbQuery.GetUserByPhone("1112223333"));
    }
}
