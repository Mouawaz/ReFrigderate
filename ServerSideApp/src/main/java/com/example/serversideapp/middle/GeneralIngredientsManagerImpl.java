package com.example.serversideapp.middle;

import Server.IngredientOuterClass;
import com.example.serversideapp.back.DBIngredientManager;
import com.example.serversideapp.shared.IngredientLocal;

import java.util.ArrayList;
import java.util.Date;
import java.util.concurrent.TimeUnit;

public class GeneralIngredientsManagerImpl implements GeneralIngredientsManager{

    private DBIngredientManager dbIngredientManager;
    public GeneralIngredientsManagerImpl(DBIngredientManager dbIngredientManager){
        this.dbIngredientManager = dbIngredientManager;
    }
    @Override
    public IngredientOuterClass.AllIngredientResponse GetAllIngredients() {
        ArrayList<IngredientOuterClass.Ingredient> subList = new ArrayList<>();
        for (IngredientLocal local : dbIngredientManager.GetAllIngredients()){
            subList.add(parseFromLocal(local));
        }
        //Since the database gives us the answer in local ingredients we first create an arraylist with the proto Ingredient
        //Then we just pass it to the Response builder
        return IngredientOuterClass.AllIngredientResponse.newBuilder()
                .addAllMessages(subList).build();
    }

    @Override
    public IngredientOuterClass.Ingredient UpdateIngredient(IngredientOuterClass.UpdateIngredientRequest request) {
        IngredientLocal local = dbIngredientManager.UpdateIngredient(request.getId(), request.getDifference(), request.getDaysUntilBad()); //IngredientId, quantity, expirationDate
        return parseFromLocal(local);
    }

    //Parses from IngredientLocal to message Ingredient
    private IngredientOuterClass.Ingredient parseFromLocal(IngredientLocal local){
        int redAmount = 5;
        int yellowAmount = 10;
        int redTime = 0;
        int yellowTime = 7;
        //in the future to be defined by the user, and read from the DB
        Date todaysDate = new Date();
        int days = (int)TimeUnit.MILLISECONDS.toDays(local.getExpirationDate().getTime() - todaysDate.getTime());
        int amountWarning = local.getAmount() <= yellowAmount ? (local.getAmount() <= redAmount ? 3 : 2) : 1;
        int timeWarning = days <= yellowTime ? (days <= redTime ? 3 : 2) : 1;
        return IngredientOuterClass.Ingredient.newBuilder()
                .setId(local.getId())
                .setName(local.getName())
                .setCost(local.getCost())
                .setAmount(local.getAmount())
                .setDaysUntilBad(days)
                .setAmountStatus(amountWarning)
                .setExpirationStatus(timeWarning).build();
    }
}
