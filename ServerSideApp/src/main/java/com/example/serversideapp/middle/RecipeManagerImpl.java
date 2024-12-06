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

    @Override
    public RecipeOuterClass.Recipe createRecipe(RecipeOuterClass.CreateRecipeRequest request) {
        ArrayList<SimplifiedIngredientLocal> ingredientLocals = new ArrayList<>();
        for (RecipeOuterClass.SimplifiedIngredient ingredient : request.getIngredientsList()) {
            ingredientLocals.add(new SimplifiedIngredientLocal(
                    ingredient.getIngredientId(),
                    ingredient.getIngredientName(),
                    ingredient.getCost(),
                    ingredient.getQuantity()
            ));
        }

        RecipeLocal newRecipeLocal = new RecipeLocal(
                0,
                request.getName(),
                request.getInstructions(),
                request.getType(),
                request.getCreatorId(),
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

        if (request.hasInstructions()) {
            existingRecipe.setInstructions(request.getInstructions());
        }

        if (request.hasType()) {
            existingRecipe.setType(request.getType());
        }

        ArrayList<SimplifiedIngredientLocal> ingredientLocals = new ArrayList<>();
        for (RecipeOuterClass.SimplifiedIngredient ingredient : request.getIngredientsList()) {
            ingredientLocals.add(new SimplifiedIngredientLocal(
                    ingredient.getIngredientId(),
                    ingredient.getIngredientName(),
                    ingredient.getQuantity(),
                    0
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
            System.out.println(sil.getQuantity());
            System.out.println(ing);
//            builder.addIngredients();
        }
        return builder.build();
    }
}