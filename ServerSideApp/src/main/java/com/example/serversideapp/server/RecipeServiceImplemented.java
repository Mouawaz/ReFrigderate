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
        RecipeOuterClass.AllRecipesResponse response = recipeManager.GetAllRecipes();
        System.out.println(response);
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void createRecipe(RecipeOuterClass.CreateRecipeRequest request, StreamObserver<RecipeOuterClass.Recipe> responseObserver) {
        RecipeOuterClass.Recipe recipe = recipeManager.createRecipe(request);
        responseObserver.onNext(recipe);
        responseObserver.onCompleted();
    }

    @Override
    public void updateRecipe(RecipeOuterClass.CreateRecipeRequest request, StreamObserver<RecipeOuterClass.Recipe> responseObserver) {
        RecipeOuterClass.Recipe recipe = recipeManager.updateRecipe(request);
        responseObserver.onNext(recipe);
        responseObserver.onCompleted();
    }

    @Override
    public void deleteRecipe(RecipeOuterClass.DeleteRecipeRequest request, StreamObserver<RecipeOuterClass.DeleteResponse> responseObserver) {
        responseObserver.onNext(RecipeOuterClass.DeleteResponse.newBuilder()
                .setIsSucces(recipeManager.deleteRecipe(request.getId())).build());
        responseObserver.onCompleted();
    }
}

