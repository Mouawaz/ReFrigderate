package com.example.serversideapp.back;

import com.example.serversideapp.shared.IngredientLocal;

import java.util.ArrayList;
import java.util.Date;

public interface DBIngredientManager {
    ArrayList<IngredientLocal> GetAllIngredients();
    IngredientLocal UpdateIngredient(int ingredientId, int quantity, int daysUntilBad);
}
