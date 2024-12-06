package com.example.serversideapp.middle;

import Server.RecipeOuterClass;
import com.example.serversideapp.shared.RecipeLocal;

import java.util.ArrayList;

public interface RecipeManager {
    RecipeOuterClass.AllRecipesResponse GetAllRecipes();
    RecipeOuterClass.Recipe createRecipe(RecipeOuterClass.CreateRecipeRequest request);
//    RecipeOuterClass.Recipe updateRecipe(RecipeOuterClass.UpdateRecipeRequest request);
    boolean deleteRecipe(int recipeId);
}
