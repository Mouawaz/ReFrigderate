package com.example.serversideapp;

//import Server.UserOuterClass;
import io.grpc.Server;
import io.grpc.ServerBuilder;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class ServerSideAppApplication {

    public static void main(String[] args) {
        int port = 8080;
        Server server = ServerBuilder.forPort(port)
                .addService(new UserServiceImpl()).build();
        try {
            server.start();
            System.out.println("Server started on port: " + port);
            server.awaitTermination();
        }
        catch (Exception e){
            System.out.println(e);
        }
//        SpringApplication.run(ServerSideAppApplication.class, args);
    }

}
