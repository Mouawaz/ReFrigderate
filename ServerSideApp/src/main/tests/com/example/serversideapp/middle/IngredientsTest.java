package com.example.serversideapp.middle;

import com.example.serversideapp.back.DBIngredientQuery;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class IngredientsTest {

    @BeforeEach
    void setUp() {
        GeneralIngredientsManager gim = new GeneralIngredientsManagerImpl(new DBIngredientQuery());
    }

    @Test
    void getAllIngredients() {

    }

    @Test
    void updateIngredient() {
    }

    @Test
    void updateWarningAmount() {
    }
}