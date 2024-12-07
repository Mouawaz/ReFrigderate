package com.example.serversideapp.back;

import Server.RecipeOuterClass;
import com.example.serversideapp.shared.RecipeLocal;
import com.example.serversideapp.shared.SimplifiedIngredientLocal;
import jdk.jshell.spi.ExecutionControl;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

public class DBRecipeQuery extends DBGeneral implements DBRecipeManager {

    @Override
    public ArrayList<RecipeLocal> GetAllIngredients() {
        try (Connection connection = getConnected()) {
            ArrayList<RecipeLocal> ans = new ArrayList<>();
            PreparedStatement psRecipes = connection.prepareStatement(
                    "SELECT recipeid, name, instructions, type, chefid, modificationsallowed FROM refridgerate.recipe"
            );
            PreparedStatement psRecipeIngredients = connection.prepareStatement(
                    "SELECT r.ingredientid, ingredient.name, ingredient.cost, quantityneeded FROM refridgerate.recipe " +
                            "JOIN refridgerate.recipeingredient r ON recipe.recipeid = r.recipeid " +
                            "JOIN refridgerate.ingredient ON r.ingredientid = ingredient.ingredientid WHERE r.recipeid=?;"
            );
            ResultSet rsRecipe = psRecipes.executeQuery();
            ArrayList<SimplifiedIngredientLocal> localIngredient;
            while (rsRecipe.next()) {
                localIngredient = new ArrayList<>();
                psRecipeIngredients.setInt(1, rsRecipe.getInt(1));
                ResultSet rsRecipeIngredients = psRecipeIngredients.executeQuery();
                while (rsRecipeIngredients.next()) {
                    localIngredient.add(new SimplifiedIngredientLocal(
                            rsRecipeIngredients.getInt(1),
                            rsRecipeIngredients.getString(2),
                            rsRecipeIngredients.getFloat(3),
                            rsRecipeIngredients.getInt(4)
                    ));
                }
                ans.add(new RecipeLocal(
                        rsRecipe.getInt(1),
                        rsRecipe.getString(2),
                        rsRecipe.getString(3),
                        rsRecipe.getString(4),
                        rsRecipe.getInt(5),
                        localIngredient,
                        rsRecipe.getBoolean(6)
                ));
            }
            return ans;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public RecipeLocal GetIngredientById(int id) {
        return GetAllIngredients().get(id - 1);
    }

    @Override
    public RecipeLocal CreateRecipe(RecipeOuterClass.CreateRecipeRequest request) {
        try (Connection connection = getConnected()) {
            PreparedStatement psQuickId = connection.prepareCall("SELECT max(recipeid) FROM refridgerate.recipe");
            PreparedStatement psCreateRecipe = connection.prepareStatement("INSERT INTO refridgerate.recipe VALUES (1, ?, ?, ?, ?, ?, CAST(? AS refridgerate.meal_course))");
            PreparedStatement psAddIngredient = connection.prepareStatement("INSERT INTO refridgerate.recipeingredient VALUES (1, ?, ?, ?)");
            ResultSet rsQuickId = psQuickId.executeQuery();
            rsQuickId.next();
            int newId = rsQuickId.getInt(1) + 1;
            psAddIngredient.setInt(1, newId);
            psCreateRecipe.setInt(1, newId);
            psCreateRecipe.setString(2, request.getName());
            psCreateRecipe.setString(3, request.getInstructions());
            psCreateRecipe.setBoolean(4, request.getModificationsAllowed());
            psCreateRecipe.setInt(5, request.getCreatorId());
            psCreateRecipe.setString(6, request.getType());
            psCreateRecipe.executeUpdate(); //Create the recipe itself, then move on to adding all the provided ingredients
            for (RecipeOuterClass.SimplifiedIngredient ing : request.getIngredientsList()) {
                psAddIngredient.setInt(2, ing.getIngredientId());
                psAddIngredient.setInt(3, ing.getQuantity());
                psAddIngredient.executeUpdate();
            }
            return GetIngredientById(newId);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public RecipeLocal UpdateRecipe(RecipeOuterClass.CreateRecipeRequest request) {
        try{
            DeleteRecipe(request.getUpdateRecipeId());
            return CreateRecipe(request);
        }catch (Exception e){
            throw new RuntimeException(e);
        }
    }

    @Override
    public boolean DeleteRecipe(int recipeId) {
        try (Connection connection = getConnected()) {
            PreparedStatement psDeleteRecipe = connection.prepareStatement(
                    "DELETE FROM refridgerate.recipe WHERE recipeid=?;"
            );
            psDeleteRecipe.setInt(1, recipeId);
            int rowsAffected = psDeleteRecipe.executeUpdate();
            return rowsAffected > 0;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}