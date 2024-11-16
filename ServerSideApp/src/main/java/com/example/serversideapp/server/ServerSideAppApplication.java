package com.example.serversideapp.server;

import com.example.serversideapp.back.DBIngredientManager;
import com.example.serversideapp.middle.GeneralIngredientsManager;
import com.example.serversideapp.middle.GeneralIngredientsManagerImpl;
import io.grpc.Server;
import io.grpc.ServerBuilder;
import com.example.serversideapp.back.DBIngredientQuery;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class ServerSideAppApplication {

    public static void main(String[] args) {
        DBIngredientManager dbIngredientManager = new DBIngredientQuery();
        GeneralIngredientsManager generalIngredientsManager = new GeneralIngredientsManagerImpl(dbIngredientManager);
        int PORT = 8080;
        Server server = ServerBuilder.forPort(PORT)
                .addService(new IngredientServiceImplemented(generalIngredientsManager)).build();
        try {
            server.start();
            System.out.println("Server started on port: " + PORT);
            server.awaitTermination();
        }
        catch (Exception e){
            System.out.println(e);
        }
    }

}
