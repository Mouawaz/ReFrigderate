package com.example.serversideapp.middle;

import Server.RecipeOuterClass;
import com.example.serversideapp.shared.RecipeLocal;

import java.util.ArrayList;

public interface RecipeManager {
    RecipeOuterClass.AllRecipesResponse GetAllRecipes();
}
