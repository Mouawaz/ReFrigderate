package com.example.serversideapp.back;

import Server.RecipeOuterClass;
import com.example.serversideapp.shared.RecipeLocal;

import java.util.ArrayList;

public interface DBRecipeManager {
    ArrayList<RecipeLocal> GetAllIngredients();
    RecipeLocal GetIngredientById(int id);

    RecipeLocal UpdateRecipe(RecipeOuterClass.CreateRecipeRequest request);

    boolean DeleteRecipe(int recipeId);

    RecipeLocal CreateRecipe(RecipeOuterClass.CreateRecipeRequest request);
}
