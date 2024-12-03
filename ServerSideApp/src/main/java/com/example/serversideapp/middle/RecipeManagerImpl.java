package com.example.serversideapp.middle;

import Server.RecipeOuterClass;
import com.example.serversideapp.back.DBRecipeManager;
import com.example.serversideapp.shared.RecipeLocal;
import com.example.serversideapp.shared.SimplifiedIngredientLocal;

import java.util.ArrayList;

public class RecipeManagerImpl implements RecipeManager {
    private DBRecipeManager dbRecipeManager;

    public RecipeManagerImpl(DBRecipeManager dbRecipeManager) {
        this.dbRecipeManager = dbRecipeManager;
    }

    @Override
    public RecipeOuterClass.AllRecipesResponse GetAllRecipes() {
        RecipeOuterClass.AllRecipesResponse.Builder respBuilder = RecipeOuterClass.AllRecipesResponse.newBuilder();
        for (RecipeLocal rl : dbRecipeManager.GetAllIngredients()){
            respBuilder.addRecipes(parseFromLocal(rl));
        }
        return respBuilder.build();
    }
    private RecipeOuterClass.Recipe parseFromLocal(RecipeLocal local){
        RecipeOuterClass.Recipe.Builder builder = RecipeOuterClass.Recipe.newBuilder()
                .setId(local.getId())
                .setName(local.getName())
                .setInstruction(local.getInstructions());
        for (SimplifiedIngredientLocal sil : local.getIngredientUsed()){
            builder.addMessages(RecipeOuterClass.SimplifiedIngredient.newBuilder()
                    .setIngredientId(sil.getId())
                    .setIngredientCost(sil.getCost())
                    .setIngredientName(sil.getName())
                    .setIngredientQuantity(sil.getQuantity()).build());
        }
        return builder.build();
    }
}
