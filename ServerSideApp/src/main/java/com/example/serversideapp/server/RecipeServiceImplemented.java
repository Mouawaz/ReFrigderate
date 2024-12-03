package com.example.serversideapp.server;

import Server.RecipeOuterClass;
import Server.RecipeServiceGrpc;
import com.example.serversideapp.middle.RecipeManager;
import io.grpc.stub.StreamObserver;

public class RecipeServiceImplemented extends RecipeServiceGrpc.RecipeServiceImplBase {
    private RecipeManager recipeManager;

    public RecipeServiceImplemented(RecipeManager recipeManager) {
        this.recipeManager = recipeManager;
    }

    @Override
    public void getAllRecipes(RecipeOuterClass.EmptyRecep request, StreamObserver<RecipeOuterClass.AllRecipesResponse> responseObserver) {
        responseObserver.onNext(recipeManager.GetAllRecipes());
        responseObserver.onCompleted();
    }
}
