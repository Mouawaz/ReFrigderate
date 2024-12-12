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
    private int yellowAmount;
    private int redAmount;
    private int yellowDays;
    private int redDays;
    private IngredientCategory category;

    public enum IngredientCategory {
        MEAT,
        POULTRY,
        VEGETABLE,
        FRUIT,
        LEGUME,
        DAIRY
    }

    public IngredientLocal(int id, String name, float cost, int amount, Date expirationDate, IngredientCategory category) {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.amount = amount;
        this.expirationDate = expirationDate;
        this.category = category;
        yellowAmount = 10;
        redAmount = 5;
        yellowDays = 7;
        redDays = 0;
    }

    public IngredientLocal(int id, String name, float cost, int amount, Date expirationDate,
                           int yellowAmount, int redAmount, int yellowDays, int redDays,
                           IngredientCategory category) {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.amount = amount;
        this.expirationDate = expirationDate;
        this.yellowAmount = yellowAmount;
        this.redAmount = redAmount;
        this.yellowDays = yellowDays;
        this.redDays = redDays;
        this.category = category;
    }
}