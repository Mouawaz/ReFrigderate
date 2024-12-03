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
    private ArrayList<SimplifiedIngredientLocal> ingredientUsed;

    public RecipeLocal(int id, String name, String instructions, ArrayList<SimplifiedIngredientLocal> ingredientUsed) {
        this.id = id;
        this.name = name;
        this.instructions = instructions;
        this.ingredientUsed = ingredientUsed;
    }
}
