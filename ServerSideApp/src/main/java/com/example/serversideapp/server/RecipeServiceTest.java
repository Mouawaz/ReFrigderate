package com.example.serversideapp.server;

import Server.RecipeOuterClass;
import com.example.serversideapp.back.DBRecipeQuery;
import com.example.serversideapp.back.DBRecipeManager;
import com.example.serversideapp.shared.RecipeLocal;
import com.example.serversideapp.shared.SimplifiedIngredientLocal;

import java.util.ArrayList;
import java.util.List;

public class RecipeServiceTest {
    private static DBRecipeManager recipeManager;

    public static void main(String[] args) {
        recipeManager = new DBRecipeQuery();
        testGetAllRecipes();
        testCreateRecipe();
        testUpdateRecipe();
        testDeleteRecipe();
    }

    private static void testGetAllRecipes() {
        System.out.println("\n--- Test Get All Recipes ---");
        try {
            List<RecipeLocal> localRecipes = recipeManager.GetAllIngredients();
            System.out.println("Total Recipes Found: " + localRecipes.size());

            for (RecipeLocal recipe : localRecipes) {
                RecipeOuterClass.Recipe protoRecipe = convertToProtoRecipe(recipe);
                System.out.println("Recipe ID: " + protoRecipe.getId());
                System.out.println("Name: " + protoRecipe.getName());
                System.out.println("Instructions: " + protoRecipe.getInstructions());
                System.out.println("Type: " + protoRecipe.getType());
                System.out.println("Creator ID: " + protoRecipe.getCreatorId());

                System.out.println("Ingredients:");
                for (RecipeOuterClass.SimplifiedIngredient ingredient : protoRecipe.getIngredientsList()) {
                    System.out.println("- Ingredient ID: " + ingredient.getIngredientId());
                    System.out.println("  Name: " + ingredient.getIngredientName());
                    System.out.println("  Quantity: " + ingredient.getQuantity());
                }
                System.out.println("---");
            }
        } catch (Exception e) {
            System.err.println("Error in GetAllRecipes: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void testCreateRecipe() {
        System.out.println("\n--- Test Create Recipe ---");
        try {
            ArrayList<SimplifiedIngredientLocal> ingredients = new ArrayList<>();
            ingredients.add(new SimplifiedIngredientLocal(1, "Tomato", 2, 0));
            ingredients.add(new SimplifiedIngredientLocal(2, "Cheese", 1, 0));

            RecipeLocal newRecipeLocal = new RecipeLocal(
                    0,
                    "Test Recipe",
                    "Test Instructions",
                    "Italian",  // type
                    1,         // creatorId
                    ingredients
            );

            RecipeLocal createdRecipe = recipeManager.CreateRecipe(newRecipeLocal);
            RecipeOuterClass.Recipe protoRecipe = convertToProtoRecipe(createdRecipe);

            System.out.println("Created Recipe:");
            System.out.println("Recipe ID: " + protoRecipe.getId());
            System.out.println("Recipe Name: " + protoRecipe.getName());
            System.out.println("Type: " + protoRecipe.getType());
            System.out.println("Creator ID: " + protoRecipe.getCreatorId());
        } catch (Exception e) {
            System.err.println("Error in CreateRecipe: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void testUpdateRecipe() {
        System.out.println("\n--- Test Update Recipe ---");
        try {
            RecipeLocal existingRecipe = recipeManager.GetRecipe(1);
            existingRecipe.setName("Updated Recipe Name");
            existingRecipe.setInstructions("Updated Instructions");
            existingRecipe.setType("Updated Type");

            RecipeLocal updatedRecipe = recipeManager.UpdateRecipe(existingRecipe);
            RecipeOuterClass.Recipe protoRecipe = convertToProtoRecipe(updatedRecipe);

            System.out.println("Updated Recipe:");
            System.out.println("Recipe ID: " + protoRecipe.getId());
            System.out.println("New Recipe Name: " + protoRecipe.getName());
            System.out.println("New Instructions: " + protoRecipe.getInstructions());
            System.out.println("New Type: " + protoRecipe.getType());
        } catch (Exception e) {
            System.err.println("Error in UpdateRecipe: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void testDeleteRecipe() {
        System.out.println("\n--- Test Delete Recipe ---");
        try {
            ArrayList<SimplifiedIngredientLocal> ingredients = new ArrayList<>();
            ingredients.add(new SimplifiedIngredientLocal(1, "Test Ingredient", 1, 0));

            RecipeLocal recipeToDelete = recipeManager.CreateRecipe(
                    new RecipeLocal(0, "Recipe to Delete", "Temporary instructions", "Test", 1, ingredients)
            );

            boolean deleteSuccess = recipeManager.DeleteRecipe(recipeToDelete.getId());
            System.out.println("Delete Recipe:");
            System.out.println("Success: " + deleteSuccess);
        } catch (Exception e) {
            System.err.println("Error in DeleteRecipe: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static RecipeOuterClass.Recipe convertToProtoRecipe(RecipeLocal recipeLocal) {
        RecipeOuterClass.Recipe.Builder recipeBuilder = RecipeOuterClass.Recipe.newBuilder()
                .setId(recipeLocal.getId())
                .setName(recipeLocal.getName())
                .setInstructions(recipeLocal.getInstructions())
                .setType(recipeLocal.getType())
                .setCreatorId(recipeLocal.getCreatorId());

        for (SimplifiedIngredientLocal ingredient : recipeLocal.getIngredientUsed()) {
            recipeBuilder.addIngredients(
                    RecipeOuterClass.SimplifiedIngredient.newBuilder()
                            .setIngredientId(ingredient.getId())
                            .setIngredientName(ingredient.getName())
                            .setQuantity(ingredient.getQuantity())
                            .build()
            );
        }

        return recipeBuilder.build();
    }
}