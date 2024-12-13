package com.example.serversideapp.shared;

import lombok.Getter;
import lombok.Setter;

@Getter @Setter
public class SimplifiedIngredientLocal {
    private int id;
    private String name;
    private float cost;
    private int quantity;
    private IngredientCategory category;

    public enum IngredientCategory {
        MEAT,
        POULTRY,
        VEGETABLE,
        FRUIT,
        LEGUME,
        DAIRY
    }

    public SimplifiedIngredientLocal(int id, String name, float cost, int quantity) {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.quantity = quantity;
    }
}
