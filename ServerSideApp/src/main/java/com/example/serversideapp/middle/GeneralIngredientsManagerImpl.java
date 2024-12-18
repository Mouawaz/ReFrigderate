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

    @Override
    public IngredientOuterClass.Success UpdateWarningAmount(IngredientOuterClass.UpdateWarningAmountsRequest request) {
        boolean ans = dbIngredientManager.UpdateWarningAmount(request.getIngredientId(), request.getYellowAmount(), request.getRedAmount(), request.getYellowDaysUntil(), request.getRedDaysUntil());
        return IngredientOuterClass.Success.newBuilder().setSuccess(ans).build();
    }

    @Override
    public IngredientOuterClass.UpdateWarningAmountsRequest getTreshold(IngredientOuterClass.IdRequest request) {
        IngredientOuterClass.UpdateWarningAmountsRequest.Builder ans = IngredientOuterClass.UpdateWarningAmountsRequest.newBuilder();
        for (IngredientLocal il : dbIngredientManager.GetAllIngredients()){
            if (il.getId()== request.getId()){
                ans
                        .setYellowAmount(il.getYellowAmount())
                        .setYellowDaysUntil(il.getYellowDays())
                        .setRedAmount(il.getRedAmount())
                        .setRedDaysUntil(il.getRedDays());
                return ans.build();
            }
        }
        throw new RuntimeException("No ingredient with id " + request.getId() + " was found");
    }

    @Override
    public IngredientOuterClass.Ingredient CreateIngredient(IngredientOuterClass.CreateIngredientRequest request) {
        return parseFromLocal(dbIngredientManager.CreateIngredient(request.getName(), request.getCategory(), request.getCost()));
    }

    //Parses from IngredientLocal to message Ingredient
    private IngredientOuterClass.Ingredient parseFromLocal(IngredientLocal local){
            int redAmount = local.getRedAmount();
            int yellowAmount = local.getYellowAmount();
            int redTime = local.getRedDays();
            int yellowTime = local.getYellowDays();
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
                    .setExpirationStatus(timeWarning)
                    .setCategory(IngredientOuterClass.IngredientCategory.valueOf(local.getCategory().name())) 
                    .build();
    }
}
