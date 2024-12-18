package com.example.serversideapp.middle;

import Server.IngredientOuterClass;
import com.example.serversideapp.back.DBIngredientQuery;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;
class IngredientsTest {
    private GeneralIngredientsManager gim;
    private IngredientOuterClass.Ingredient test;
    @BeforeEach
    void setUp() {
        gim = new GeneralIngredientsManagerImpl(new DBIngredientQuery());
        test = IngredientOuterClass.Ingredient.newBuilder()
                .setId(0)
                .setName("Test")
                .setCost(0.5f)
                .setAmount(11)
                .setDaysUntilBad(-3)
                .setAmountStatus(1)
                .setExpirationStatus(3)
                .build();
    }

    @Test
    void updateIngredient() {
        assertDoesNotThrow(()->{
            gim.UpdateIngredient(IngredientOuterClass.UpdateIngredientRequest.newBuilder()
                    .setId(0)
                    .setDifference(-11)
                    .setDaysUntilBad(0).build());
            gim.UpdateIngredient(IngredientOuterClass.UpdateIngredientRequest.newBuilder()
                    .setId(0)
                    .setDifference(11)
                    .setDaysUntilBad(0).build());
        });
    }

    @Test
    void getAllIngredients() {
        IngredientOuterClass.AllIngredientResponse resp = gim.GetAllIngredients();
        assertEquals(resp.getMessages(0), test);
    }

    @Test
    void updateWarningAmount() {
        assertEquals(gim.GetAllIngredients().getMessages(0).getAmountStatus(), 1);
        assertDoesNotThrow(()->{
            gim.UpdateWarningAmount(IngredientOuterClass.UpdateWarningAmountsRequest.newBuilder()
                    .setRedAmount(11)
                    .setYellowAmount(12).build());
        });
        assertEquals(gim.GetAllIngredients().getMessages(0).getAmountStatus(), 3);
        assertDoesNotThrow(()->{
            gim.UpdateWarningAmount(IngredientOuterClass.UpdateWarningAmountsRequest.newBuilder()
                    .setRedAmount(5)
                    .setYellowAmount(10)
                    .setRedDaysUntil(0)
                    .setYellowAmount(7).build());
        });
    }
    @Test
    void createIngredient(){
        assertDoesNotThrow(()->{
            gim.CreateIngredient(IngredientOuterClass.CreateIngredientRequest.newBuilder()
                    .setName("Sauce")
                    .setCategory("Legume")
                    .setCost(2.0f).build());

        });
    }
    @AfterEach
    void printOut(){
        System.out.println(gim.GetAllIngredients().getMessagesCount());
        System.out.println(gim.GetAllIngredients());
        System.out.println("__________________________________");
    }
}