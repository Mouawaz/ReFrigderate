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

    public IngredientLocal(int id, String name, float cost, int amount, Date expirationDate) {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.amount = amount;
        this.expirationDate = expirationDate;
        yellowAmount = 10;
        redAmount = 5;
        yellowDays = 7;
        redDays = 0;
    }

    public IngredientLocal(int id, String name, float cost, int amount, Date expirationDate, int yellowAmount, int redAmount, int yellowDays, int redDays) {
        this.id = id;
        this.name = name;
        this.cost = cost;
        this.amount = amount;
        this.expirationDate = expirationDate;
        this.yellowAmount = yellowAmount;
        this.redAmount = redAmount;
        this.yellowDays = yellowDays;
        this.redDays = redDays;
    }
}
