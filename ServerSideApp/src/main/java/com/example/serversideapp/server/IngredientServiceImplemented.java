package com.example.serversideapp.server;

import Server.IngredientOuterClass;
import Server.IngredientServiceGrpc;
import com.example.serversideapp.middle.GeneralIngredientsManager;
import io.grpc.stub.StreamObserver;

public class IngredientServiceImplemented extends IngredientServiceGrpc.IngredientServiceImplBase {

    private GeneralIngredientsManager generalIngredientsManager;
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

    @Override
    public void updateWarningAmount(IngredientOuterClass.UpdateWarningAmountsRequest request, StreamObserver<IngredientOuterClass.Success> responseObserver) {
        responseObserver.onNext(generalIngredientsManager.UpdateWarningAmount(request));
        responseObserver.onCompleted();
    }

    @Override
    public void getTreshold(IngredientOuterClass.IdRequest request, StreamObserver<IngredientOuterClass.UpdateWarningAmountsRequest> responseObserver) {
        try {
            responseObserver.onNext(generalIngredientsManager.getTreshold(request));
            responseObserver.onCompleted();
        }
        catch (Exception e){
            responseObserver.onError(e);
        }
    }

    @Override
    public void createIngredient(IngredientOuterClass.CreateIngredientRequest request, StreamObserver<IngredientOuterClass.Ingredient> responseObserver) {
        try {
            responseObserver.onNext(generalIngredientsManager.CreateIngredient(request));
            responseObserver.onCompleted();
        }
        catch (Exception e){
            System.out.println(e);
            responseObserver.onError(e);
        }
    }
}
