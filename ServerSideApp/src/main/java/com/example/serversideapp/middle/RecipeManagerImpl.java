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
//        System.out.println(respBuilder.build());
        return respBuilder.build();
    }

    @Override
    public RecipeOuterClass.Recipe createRecipe(RecipeOuterClass.CreateRecipeRequest request) {
        RecipeLocal savedRecipe = dbRecipeManager.CreateRecipe(request);
        return parseFromLocal(savedRecipe);
    }

    @Override
    public RecipeOuterClass.Recipe updateRecipe(RecipeOuterClass.CreateRecipeRequest request) {
        RecipeLocal updatedRecipe = dbRecipeManager.UpdateRecipe(request);
        return parseFromLocal(updatedRecipe);
    }

    @Override
    public boolean deleteRecipe(int recipeId) {
        return dbRecipeManager.DeleteRecipe(recipeId);
    }

    private RecipeOuterClass.Recipe parseFromLocal(RecipeLocal local){
        RecipeOuterClass.Recipe.Builder builder = RecipeOuterClass.Recipe.newBuilder()
                .setId(local.getId())
                .setName(local.getName())
                .setModificationsAllowed(local.isModsAllowed())
                .setInstructions(local.getInstructions())
                .setType(local.getType())
                .setCreatorId(local.getCreatorId());
        for (SimplifiedIngredientLocal sil : local.getIngredientUsed()){
            RecipeOuterClass.SimplifiedIngredient ing = RecipeOuterClass.SimplifiedIngredient.newBuilder()
                    .setIngredientId(sil.getId())
                    .setIngredientName(sil.getName())
                    .setQuantity(sil.getQuantity())
                    .setCost(sil.getCost())
                    .build();
            builder.addIngredients(ing);
        }
        return builder.build();
    }
}