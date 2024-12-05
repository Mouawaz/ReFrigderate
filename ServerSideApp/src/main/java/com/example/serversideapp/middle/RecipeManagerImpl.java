package com.example.serversideapp.middle;

import Server.RecipeOuterClass;
import com.example.serversideapp.back.DBRecipeManager;
import com.example.serversideapp.shared.RecipeLocal;
import com.example.serversideapp.shared.SimplifiedIngredientLocal;

import java.util.ArrayList;
import java.util.List;

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

    @Override
    public RecipeOuterClass.Recipe createRecipe(RecipeOuterClass.CreateRecipeRequest request) {
        // Convert ingredients from protobuf to local using ArrayList
        ArrayList<SimplifiedIngredientLocal> ingredientLocals = new ArrayList<>();
        for (RecipeOuterClass.SimplifiedIngredient ingredient : request.getIngredientsList()) {
            ingredientLocals.add(new SimplifiedIngredientLocal(
                    ingredient.getIngredientId(),
                    ingredient.getIngredientName(),
                    ingredient.getIngredientQuantity(),
                    (int) ingredient.getIngredientCost()
            ));
        }


        RecipeLocal newRecipeLocal = new RecipeLocal(
                0,
                request.getName(),
                request.getInstruction(),
                ingredientLocals
        );

        RecipeLocal savedRecipe = dbRecipeManager.CreateRecipe(newRecipeLocal);


        return parseFromLocal(savedRecipe);
    }

    @Override
    public RecipeOuterClass.Recipe updateRecipe(RecipeOuterClass.UpdateRecipeRequest request) {
        RecipeLocal existingRecipe = dbRecipeManager.GetRecipe(request.getId());

        if (existingRecipe == null) {
            throw new IllegalArgumentException("Recipe not found with ID: " + request.getId());
        }

        if (request.hasName()) {
            existingRecipe.setName(request.getName());
        }

        if (request.hasInstruction()) {
            existingRecipe.setInstructions(request.getInstruction());
        }

        ArrayList<SimplifiedIngredientLocal> ingredientLocals = new ArrayList<>();
        for (RecipeOuterClass.SimplifiedIngredient ingredient : request.getIngredientsList()) {
            ingredientLocals.add(new SimplifiedIngredientLocal(
                    ingredient.getIngredientId(),
                    ingredient.getIngredientName(),
                    ingredient.getIngredientQuantity(),
                    (int) ingredient.getIngredientCost()
            ));
        }
        existingRecipe.setIngredientUsed(ingredientLocals);


        RecipeLocal updatedRecipe = dbRecipeManager.UpdateRecipe(existingRecipe);

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
                .setInstruction(local.getInstructions());

        for (SimplifiedIngredientLocal sil : local.getIngredientUsed()){
            builder.addIngredients(RecipeOuterClass.SimplifiedIngredient.newBuilder()
                    .setIngredientId(sil.getId())
                    .setIngredientCost((float) sil.getCost())
                    .setIngredientName(sil.getName())
                    .setIngredientQuantity(sil.getQuantity()).build());
        }
        return builder.build();
    }
}