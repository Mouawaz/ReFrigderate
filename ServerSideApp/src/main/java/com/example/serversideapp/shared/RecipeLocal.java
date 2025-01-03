package com.example.serversideapp.shared;

import lombok.Getter;
import lombok.Setter;

import java.util.ArrayList;

@Setter
@Getter
public class RecipeLocal {
    private int id;
    private String name;
    private String instructions;
    private String type;
    private int creatorId;
    private boolean modsAllowed;
    private ArrayList<SimplifiedIngredientLocal> ingredientUsed;


    public RecipeLocal(int id, String name, String instructions, String type, int creatorId, ArrayList<SimplifiedIngredientLocal> ingredientUsed, boolean modsAllowed) {
        this.id = id;
        this.name = name;
        this.instructions = instructions;
        this.type = type;
        this.creatorId = creatorId;
        this.ingredientUsed = ingredientUsed;
        this.modsAllowed = modsAllowed;
    }
}