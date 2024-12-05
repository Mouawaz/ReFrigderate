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
        // Initialize the recipe manager
        recipeManager = new DBRecipeQuery();

        // Run all test methods
        testGetAllRecipes();
        testCreateRecipe();
        testUpdateRecipe();
        testDeleteRecipe();
    }

    private static void testGetAllRecipes() {
        System.out.println("\n--- Test Get All Recipes ---");
        try {
            // Create the AllRecipesResponse equivalent
            List<RecipeLocal> localRecipes = recipeManager.GetAllIngredients();

            System.out.println("Total Recipes Found: " + localRecipes.size());

            for (RecipeLocal recipe : localRecipes) {
                // Simulate converting local recipe to protobuf Recipe
                RecipeOuterClass.Recipe protoRecipe = convertToProtoRecipe(recipe);

                System.out.println("Recipe ID: " + protoRecipe.getId());
                System.out.println("Name: " + protoRecipe.getName());
                System.out.println("Instructions: " + protoRecipe.getInstruction());

                System.out.println("Ingredients:");
                for (RecipeOuterClass.SimplifiedIngredient ingredient : protoRecipe.getIngredientsList()) {
                    System.out.println("- Ingredient ID: " + ingredient.getIngredientId());
                    System.out.println("  Name: " + ingredient.getIngredientName());
                    System.out.println("  Quantity: " + ingredient.getIngredientQuantity());
                    System.out.println("  Cost: " + ingredient.getIngredientCost());
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
            // Prepare a CreateRecipeRequest equivalent
            List<SimplifiedIngredientLocal> ingredients = new ArrayList<>();
            ingredients.add(new SimplifiedIngredientLocal(1, "Tomato", 0.5f, 2));
            ingredients.add(new SimplifiedIngredientLocal(2, "Cheese", 2.0f, 1));

            RecipeLocal newRecipeLocal = new RecipeLocal(0, "Test Recipe", "Test Instructions", (ArrayList<SimplifiedIngredientLocal>) ingredients);

            // Create the recipe
            RecipeLocal createdRecipe = recipeManager.CreateRecipe(newRecipeLocal);

            // Convert to protobuf for verification
            RecipeOuterClass.Recipe protoRecipe = convertToProtoRecipe(createdRecipe);

            // Prepare RecipeResponse
            RecipeOuterClass.RecipeResponse response = RecipeOuterClass.RecipeResponse.newBuilder()
                    .setSuccess(true)
                    .setMessage("Recipe created successfully")
                    .setRecipe(protoRecipe)
                    .build();

            System.out.println("Created Recipe:");
            System.out.println("Success: " + response.getSuccess());
            System.out.println("Message: " + response.getMessage());
            System.out.println("Recipe ID: " + response.getRecipe().getId());
            System.out.println("Recipe Name: " + response.getRecipe().getName());
        } catch (Exception e) {
            System.err.println("Error in CreateRecipe: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void testUpdateRecipe() {
        System.out.println("\n--- Test Update Recipe ---");
        try {
            // Assume we're updating an existing recipe (ID 1)
            RecipeLocal existingRecipe = recipeManager.GetRecipe(1);

            // Modify the recipe
            existingRecipe.setName("Updated Recipe Name");
            existingRecipe.setInstructions("Updated Instructions");

            // Update the recipe
            RecipeLocal updatedRecipe = recipeManager.UpdateRecipe(existingRecipe);

            // Convert to protobuf for verification
            RecipeOuterClass.Recipe protoRecipe = convertToProtoRecipe(updatedRecipe);

            // Prepare RecipeResponse
            RecipeOuterClass.RecipeResponse response = RecipeOuterClass.RecipeResponse.newBuilder()
                    .setSuccess(true)
                    .setMessage("Recipe updated successfully")
                    .setRecipe(protoRecipe)
                    .build();

            System.out.println("Updated Recipe:");
            System.out.println("Success: " + response.getSuccess());
            System.out.println("Message: " + response.getMessage());
            System.out.println("Recipe ID: " + response.getRecipe().getId());
            System.out.println("New Recipe Name: " + response.getRecipe().getName());
            System.out.println("New Instructions: " + response.getRecipe().getInstruction());
        } catch (Exception e) {
            System.err.println("Error in UpdateRecipe: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void testDeleteRecipe() {
        System.out.println("\n--- Test Delete Recipe ---");
        try {
            // Create a recipe to delete
            ArrayList<SimplifiedIngredientLocal> ingredients = new ArrayList<>();
            ingredients.add(new SimplifiedIngredientLocal(1, "Test Ingredient", 1.0f, 2));

            RecipeLocal recipeToDelete = recipeManager.CreateRecipe(
                    new RecipeLocal(0, "Recipe to Delete", "Temporary instructions", ingredients)
            );

            // Delete the recipe
            boolean deleteSuccess = recipeManager.DeleteRecipe(recipeToDelete.getId());

            // Prepare RecipeResponse
            RecipeOuterClass.RecipeResponse response = RecipeOuterClass.RecipeResponse.newBuilder()
                    .setSuccess(deleteSuccess)
                    .setMessage(deleteSuccess ? "Recipe deleted successfully" : "Failed to delete recipe")
                    .build();

            System.out.println("Delete Recipe:");
            System.out.println("Success: " + response.getSuccess());
            System.out.println("Message: " + response.getMessage());
        } catch (Exception e) {
            System.err.println("Error in DeleteRecipe: " + e.getMessage());
            e.printStackTrace();
        }
    }

    // Utility method to convert RecipeLocal to Protobuf Recipe
    private static RecipeOuterClass.Recipe convertToProtoRecipe(RecipeLocal recipeLocal) {
        RecipeOuterClass.Recipe.Builder recipeBuilder = RecipeOuterClass.Recipe.newBuilder()
                .setId(recipeLocal.getId())
                .setName(recipeLocal.getName())
                .setInstruction(recipeLocal.getInstructions());

        for (SimplifiedIngredientLocal ingredient : recipeLocal.getIngredientUsed()) {
            recipeBuilder.addIngredients(
                    RecipeOuterClass.SimplifiedIngredient.newBuilder()
                            .setIngredientId(ingredient.getId())
                            .setIngredientName(ingredient.getName())
                            .setIngredientQuantity(ingredient.getQuantity())
                            .setIngredientCost(ingredient.getCost())
                            .build()
            );
        }

        return recipeBuilder.build();
    }
}
