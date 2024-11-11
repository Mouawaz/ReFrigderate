package com.example.serversideapp.server;

//import Server.UserOuterClass;
import com.example.serversideapp.back.DBQuery;
import com.example.serversideapp.middle.GeneralIngredientsManager;
import com.example.serversideapp.middle.GeneralIngredientsManagerImpl;
import io.grpc.Server;
import io.grpc.ServerBuilder;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class ServerSideAppApplication {

    public static void main(String[] args) {
        DBQuery dbQuery = new DBQuery();
        GeneralIngredientsManager ingredientsManager = new GeneralIngredientsManagerImpl(dbQuery);
        int PORT = 8080;
        Server server = ServerBuilder.forPort(PORT)
                .addService(new IngredientServiceImplemented(ingredientsManager)).build();
        try {
            server.start();
            System.out.println("Server started on port: " + PORT);
            server.awaitTermination();
        }
        catch (Exception e){
            System.out.println(e);
        }
//        SpringApplication.run(ServerSideAppApplication.class, args);
    }

}
