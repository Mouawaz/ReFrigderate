package com.example.serversideapp.server;

import com.example.serversideapp.back.DBIngredientManager;
import com.example.serversideapp.back.DBRecipeQuery;
import com.example.serversideapp.back.DBUserQuery;
import com.example.serversideapp.middle.*;
import io.grpc.Server;
import io.grpc.ServerBuilder;
import com.example.serversideapp.back.DBIngredientQuery;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import Server.User;

@SpringBootApplication
public class ServerSideAppApplication {
    public static void main(String[] args) {
//        UserManager userManager = new UserManagerImpl(new DBUserQuery());
//        System.out.println(userManager.AddUser(User.CreateUserRequest.newBuilder()
//                .setEmail("DPKK@example.com")
//                .setFirstname("Kurcze")
//                .setLastname("Blade")
//                .setPassword("2137").build()));
        //--------------------------------------Tests above
        GeneralIngredientsManager generalIngredientsManager = new GeneralIngredientsManagerImpl(new DBIngredientQuery());
        UserManager userManager = new UserManagerImpl(new DBUserQuery());
        RecipeManager recipeManager = new RecipeManagerImpl(new DBRecipeQuery());
        //create all the managers please
        int PORT = 8080;
        Server server = ServerBuilder.forPort(PORT)
                .addService(new IngredientServiceImplemented(generalIngredientsManager))
                .addService(new UserServiceImplemented(userManager))
                .addService(new RecipeServiceImplemented(recipeManager)).build();
        try {
            server.start();
            System.out.println("Server started on port: " + PORT);
            server.awaitTermination();
        } catch (Exception e) {
            System.out.println(e);
        }
    }

}
