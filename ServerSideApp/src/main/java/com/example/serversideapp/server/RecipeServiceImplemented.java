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
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }


    @Override
    public void createRecipe(RecipeOuterClass.CreateRecipeRequest request, StreamObserver<RecipeOuterClass.RecipeResponse> responseObserver) {
        RecipeOuterClass.Recipe recipe = recipeManager.createRecipe(request);
        RecipeOuterClass.RecipeResponse response = RecipeOuterClass.RecipeResponse.newBuilder()
                .setSuccess(true)
                .setRecipe(recipe)
                .build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override public void updateRecipe(RecipeOuterClass.UpdateRecipeRequest request, StreamObserver<RecipeOuterClass.RecipeResponse> responseObserver) {
        RecipeOuterClass.Recipe recipe = recipeManager.updateRecipe(request);
        RecipeOuterClass.RecipeResponse response = RecipeOuterClass.RecipeResponse.newBuilder()
                .setSuccess(true)
                .setRecipe(recipe)
                .build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }

    @Override
    public void deleteRecipe(RecipeOuterClass.DeleteRecipeRequest request, StreamObserver<RecipeOuterClass.RecipeResponse> responseObserver) {
        boolean deleted = recipeManager.deleteRecipe(request.getId());
        RecipeOuterClass.RecipeResponse response = RecipeOuterClass.RecipeResponse.newBuilder()
                .setSuccess(deleted)
                .build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();
    }
}

