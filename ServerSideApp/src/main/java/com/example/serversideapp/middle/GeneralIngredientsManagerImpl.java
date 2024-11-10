package com.example.serversideapp.middle;

import Server.IngredientOuterClass;
import com.example.serversideapp.back.DBManager;
import com.example.serversideapp.shared.IngredientLocal;

import java.util.ArrayList;
import java.util.Date;

public class GeneralIngredientsManagerImpl implements GeneralIngredientsManager{

    DBManager dbManager;
    public GeneralIngredientsManagerImpl(DBManager dbManager){
        this.dbManager = dbManager;
    }
    @Override
    public IngredientOuterClass.AllIngredientResponse GetAllIngredients() {
        ArrayList<IngredientOuterClass.Ingredient> subList = new ArrayList<>();
        for (IngredientLocal local : dbManager.GetAllIngredients()){
            subList.add(parseFromLocal(local));
        }
        //Since the database gives us the answer in local ingredients we first create an arraylist with the proto Ingredient
        //Then we just pass it to the Response builder
        return IngredientOuterClass.AllIngredientResponse.newBuilder()
                .addAllMessages(subList).build();
    }
    //Parses from IngredientLocal to message Ingredient
    private IngredientOuterClass.Ingredient parseFromLocal(IngredientLocal local){
        Date TodaysDate = new Date();
        return IngredientOuterClass.Ingredient.newBuilder()
                .setId(local.getId())
                .setName(local.getName())
                .setCost(local.getCost())
                .setAmount(local.getAmount())
                .setDaysUntilBad(local.getExpirationDate().compareTo(TodaysDate)).build();
    }
}
