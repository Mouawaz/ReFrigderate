package com.example.serversideapp.middle;

import com.example.serversideapp.back.DBRecipeQuery;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class RecipeTest {
    private RecipeManager rm;
    @BeforeEach
    void setUp() {
        rm = new RecipeManagerImpl(new DBRecipeQuery());
    }

    @Test
    void getAllRecipes() {

    }

    @Test
    void createRecipe() {
    }

    @Test
    void updateRecipe() {
    }

    @Test
    void deleteRecipe() {
    }
}