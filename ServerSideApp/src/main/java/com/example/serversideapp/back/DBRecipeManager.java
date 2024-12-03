package com.example.serversideapp.back;

import com.example.serversideapp.shared.RecipeLocal;

import java.util.ArrayList;

public interface DBRecipeManager {
    ArrayList<RecipeLocal> GetAllIngredients();
    RecipeLocal GetIngredientById(int id);
}
