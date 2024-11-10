package com.example.serversideapp.shared;

import lombok.Getter;
import lombok.Setter;

import java.util.Date;
@Getter @Setter
public class IngredientLocal {
    private int id;
    private String name;
    private float cost;
    private int amount;
    private Date expirationDate;

    public IngredientLocal(int id, String name, float cost, int amount, Date expirationDate) {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.amount = amount;
        this.expirationDate = expirationDate;
    }
}
