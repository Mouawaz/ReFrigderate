package com.example.serversideapp.middle;

import Server.RecipeOuterClass;
import com.example.serversideapp.back.DBRecipeQuery;
import org.junit.jupiter.api.*;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
class RecipeTest {
    private RecipeManager rm;
    private RecipeOuterClass.Recipe test;

    @BeforeEach
    void setUp() {
        rm = new RecipeManagerImpl(new DBRecipeQuery());
        test = RecipeOuterClass.Recipe.newBuilder()
                .setId(0)
                .setName("Test")
                .setType("Starter")
                .setInstructions("TestInst")
                .setModificationsAllowed(false)
                .setCreatorId(1)
                .addIngredients(RecipeOuterClass.SimplifiedIngredient.newBuilder()
                        .setIngredientName("Test")
                        .setIngredientId(0)
                        .setQuantity(5)
                        .setCost(0.5f).build()).build();

    }

    @Test
    @Order(1)
    void getAllRecipes() {
        assertEquals(rm.GetAllRecipes().getRecipes(0), test);
    }

    RecipeOuterClass.Recipe localCreateAns;
    @Test
    @Order(2)
    void createRecipe() {
        assertDoesNotThrow(()->{
            localCreateAns = rm.createRecipe(RecipeOuterClass.CreateRecipeRequest.newBuilder()
                    .setName("Test2")
                    .setInstructions("Test2Inst")
                    .setModificationsAllowed(true)
                    .setCreatorId(1)
                    .setType("Main")
                    .addIngredients(RecipeOuterClass.SimplifiedIngredient.newBuilder()
                            .setIngredientId(0)
                            .setQuantity(20).build()).build());
        });
        assertEquals(rm.GetAllRecipes().getRecipes(localCreateAns.getId()).getName(),"Test2");
    }

    @Test
    @Order(3)
    void updateRecipe() {
        int index = rm.GetAllRecipes().getRecipesCount();
        String ogName = rm.GetAllRecipes().getRecipes(index-2).getName();
        localCreateAns = rm.updateRecipe(RecipeOuterClass.CreateRecipeRequest.newBuilder()
                .setName("Test3")
                .setInstructions("Test3Inst")
                .setModificationsAllowed(true)
                .setCreatorId(1)
                .setType("Main")
                .addIngredients(RecipeOuterClass.SimplifiedIngredient.newBuilder()
                        .setIngredientId(0)
                        .setQuantity(25).build())
                .setUpdateRecipeId(index-1).build());
        assertEquals(rm.GetAllRecipes().getRecipes(index-1).getName(),"Test3");
        assertEquals(rm.GetAllRecipes().getRecipes(index-2).getName(),ogName);
    }

    @Test
    @Order(4)
    void deleteRecipe() {
        int index = rm.GetAllRecipes().getRecipesCount();
        assertDoesNotThrow(()->{
            rm.deleteRecipe(index-1);
        });
        assertTrue(rm.GetAllRecipes().getRecipesCount() < index);
    }
    @AfterEach
    void printOut(){
        System.out.println(rm.GetAllRecipes().getRecipesCount());
        System.out.println(rm.GetAllRecipes());
        System.out.println("__________________________________");
    }
}