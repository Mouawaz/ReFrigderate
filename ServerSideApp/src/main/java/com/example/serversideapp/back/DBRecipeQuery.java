package com.example.serversideapp.back;

import com.example.serversideapp.shared.RecipeLocal;
import com.example.serversideapp.shared.SimplifiedIngredientLocal;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

public class DBRecipeQuery extends DBGeneral implements DBRecipeManager {

    @Override
    public ArrayList<RecipeLocal> GetAllIngredients() {
        try(Connection connection = getConnected()){
            ArrayList<RecipeLocal> ans = new ArrayList<>();
            PreparedStatement psRecipes = connection.prepareStatement("SELECT recipeid, name, instructions, modificationsallowed FROM refridgerate.recipe");
            PreparedStatement psRecipeIngredients = connection.prepareStatement("SELECT quantityneeded, r.ingredientid, ingredient.name, ingredient.cost FROM refridgerate.recipe " +
                    "JOIN refridgerate.recipeingredient r ON recipe.recipeid = r.recipeid " +
                    "JOIN refridgerate.ingredient ON r.ingredientid = ingredient.ingredientid WHERE r.recipeid=?;");
            ResultSet rsRecipe = psRecipes.executeQuery();
            ArrayList<SimplifiedIngredientLocal> localIngredient;
            while (rsRecipe.next()){
                localIngredient = new ArrayList<>(); //ArrayList for all of the ingredient in a given recipe
                psRecipeIngredients.setInt(1, rsRecipe.getInt(1));
                ResultSet rsRecipeIngredients = psRecipeIngredients.executeQuery();
                while (rsRecipeIngredients.next()){
                    localIngredient.add(new SimplifiedIngredientLocal(rsRecipeIngredients.getInt(2), rsRecipeIngredients.getString(3), rsRecipeIngredients.getFloat(4), rsRecipeIngredients.getInt(1)));
                }
                ans.add(new RecipeLocal(rsRecipe.getInt(1), rsRecipe.getString(2), rsRecipe.getString(3), localIngredient));
            }
            return ans;
        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }

    @Override
    public RecipeLocal GetIngredientById(int id) {
        return null;
    }
}
