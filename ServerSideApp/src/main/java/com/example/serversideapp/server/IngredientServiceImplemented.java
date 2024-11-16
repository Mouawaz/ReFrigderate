package com.example.serversideapp.server;

import Server.IngredientOuterClass;
import Server.IngredientServiceGrpc;
import com.example.serversideapp.middle.GeneralIngredientsManager;
import io.grpc.stub.StreamObserver;

public class IngredientServiceImplemented extends IngredientServiceGrpc.IngredientServiceImplBase {

    GeneralIngredientsManager generalIngredientsManager;
    public IngredientServiceImplemented(GeneralIngredientsManager generalIngredientsManager){
        this.generalIngredientsManager = generalIngredientsManager;
    }
    //Simply calls the getAllIngredients from the manager :)
    @Override
    public void getAllIngredients(IngredientOuterClass.Empty request, StreamObserver<IngredientOuterClass.AllIngredientResponse> responseObserver) {
        responseObserver.onNext(generalIngredientsManager.GetAllIngredients());
        responseObserver.onCompleted();
    }

    @Override
    public void updateIngredient(IngredientOuterClass.UpdateIngredientRequest request, StreamObserver<IngredientOuterClass.Ingredient> responseObserver) {
        responseObserver.onNext(generalIngredientsManager.UpdateIngredient(request));
        responseObserver.onCompleted();
    }
}
