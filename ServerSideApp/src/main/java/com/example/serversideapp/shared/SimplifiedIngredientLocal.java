package com.example.serversideapp.shared;

import lombok.Getter;
import lombok.Setter;

import java.util.Date;
@Getter @Setter
public class SimplifiedIngredientLocal {
    private int id;
    private String name;
    private float cost;
    private int quantity;

    public SimplifiedIngredientLocal(int id, String name, float cost, int quantity) {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.quantity = quantity;
    }
}
