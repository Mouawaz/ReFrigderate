package com.example.serversideapp.middle;

import Server.IngredientOuterClass;

import java.util.ArrayList;

public interface GeneralIngredientsManager {
    IngredientOuterClass.AllIngredientResponse GetAllIngredients();
    IngredientOuterClass.Ingredient UpdateIngredient(IngredientOuterClass.UpdateIngredientRequest request);
    IngredientOuterClass.Ingredient CreateIngredient(IngredientOuterClass.CreateIngredientRequest request);
    IngredientOuterClass.Success UpdateWarningAmount(IngredientOuterClass.UpdateWarningAmountsRequest request);
    IngredientOuterClass.UpdateWarningAmountsRequest getTreshold(IngredientOuterClass.IdRequest request);
}
