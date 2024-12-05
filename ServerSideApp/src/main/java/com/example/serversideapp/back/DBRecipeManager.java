package com.example.serversideapp.back;

import com.example.serversideapp.shared.RecipeLocal;

import java.util.ArrayList;

public interface DBRecipeManager {
    ArrayList<RecipeLocal> GetAllIngredients();
    RecipeLocal GetIngredientById(int id);

    RecipeLocal GetRecipe(int id);

    RecipeLocal UpdateRecipe(RecipeLocal existingRecipe);

    boolean DeleteRecipe(int recipeId);

    RecipeLocal CreateRecipe(RecipeLocal newRecipeLocal);
}
