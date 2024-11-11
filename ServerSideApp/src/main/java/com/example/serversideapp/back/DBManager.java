package com.example.serversideapp.back;

import com.example.serversideapp.shared.IngredientLocal;

import java.util.ArrayList;

public interface DBManager {
    ArrayList<IngredientLocal> GetAllIngredients();
}
