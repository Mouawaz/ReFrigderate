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
        try (Connection connection = getConnected()) {
            ArrayList<RecipeLocal> ans = new ArrayList<>();
            PreparedStatement psRecipes = connection.prepareStatement(
                    "SELECT recipeid, name, instructions, type, chefid FROM refridgerate.recipe"
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
                        localIngredient
                ));
            }
            return ans;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public RecipeLocal GetIngredientById(int id) {
        return GetRecipe(id);
    }

    @Override
    public RecipeLocal GetRecipe(int id) {
        try (Connection connection = getConnected()) {
            PreparedStatement psRecipe = connection.prepareStatement(
                    "SELECT recipeid, name, instructions, type, chefid FROM refridgerate.recipe WHERE recipeid = ?"
            );
            PreparedStatement psRecipeIngredients = connection.prepareStatement(
                    "SELECT quantityneeded, r.ingredientid, ingredient.name, ingredient.cost FROM refridgerate.recipe " +
                            "JOIN refridgerate.recipeingredient r ON recipe.recipeid = r.recipeid " +
                            "JOIN refridgerate.ingredient ON r.ingredientid = ingredient.ingredientid WHERE r.recipeid = ?"
            );

            psRecipe.setInt(1, id);
            psRecipeIngredients.setInt(1, id);

            ResultSet rsRecipe = psRecipe.executeQuery();

            if (rsRecipe.next()) {
                ArrayList<SimplifiedIngredientLocal> localIngredient = new ArrayList<>();
                ResultSet rsRecipeIngredients = psRecipeIngredients.executeQuery();

                while (rsRecipeIngredients.next()) {
                    localIngredient.add(new SimplifiedIngredientLocal(
                            rsRecipeIngredients.getInt(2),
                            rsRecipeIngredients.getString(3),
                            rsRecipeIngredients.getInt(4),
                            rsRecipeIngredients.getInt(1)
                    ));
                }

                return new RecipeLocal(
                        rsRecipe.getInt(1),
                        rsRecipe.getString(2),
                        rsRecipe.getString(3),
                        rsRecipe.getString(4),
                        rsRecipe.getInt(5),
                        localIngredient
                );
            }

            return null;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public RecipeLocal CreateRecipe(RecipeLocal newRecipeLocal) {
        try (Connection connection = getConnected()) {
            PreparedStatement psCreateRecipe = connection.prepareStatement(
                    "INSERT INTO refridgerate.recipe (name, instructions, type, chefid) VALUES (?, ?, ?, ?) RETURNING recipeid"
            );

            psCreateRecipe.setString(1, newRecipeLocal.getName());
            psCreateRecipe.setString(2, newRecipeLocal.getInstructions());
            psCreateRecipe.setString(3, newRecipeLocal.getType());
            psCreateRecipe.setInt(4, newRecipeLocal.getCreatorId());

            ResultSet rs = psCreateRecipe.executeQuery();

            int newRecipeId = -1;
            if (rs.next()) {
                newRecipeId = rs.getInt(1);
            }

            PreparedStatement psInsertIngredient = connection.prepareStatement(
                    "INSERT INTO refridgerate.RecipeIngredient (recipeid, ingredientid, quantityneeded) VALUES (?, ?, ?)"
            );

            for (SimplifiedIngredientLocal ingredient : newRecipeLocal.getIngredientUsed()) {
                psInsertIngredient.setInt(1, newRecipeId);
                psInsertIngredient.setInt(2, ingredient.getId());
                psInsertIngredient.setInt(3, ingredient.getQuantity());
                psInsertIngredient.executeUpdate();
            }

            return GetRecipe(newRecipeId);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public RecipeLocal UpdateRecipe(RecipeLocal existingRecipe) {
        try (Connection connection = getConnected()) {
            PreparedStatement psUpdateRecipe = connection.prepareStatement(
                    "UPDATE refridgerate.recipe SET name = ?, instructions = ?, type = ?, chefid = ? WHERE recipeid = ?"
            );

            psUpdateRecipe.setString(1, existingRecipe.getName());
            psUpdateRecipe.setString(2, existingRecipe.getInstructions());
            psUpdateRecipe.setString(3, existingRecipe.getType());
            psUpdateRecipe.setInt(4, existingRecipe.getCreatorId());
            psUpdateRecipe.setInt(5, existingRecipe.getId());
            psUpdateRecipe.executeUpdate();

            PreparedStatement psDeleteIngredients = connection.prepareStatement(
                    "DELETE FROM refridgerate.recipeingredient WHERE recipeid = ?"
            );
            psDeleteIngredients.setInt(1, existingRecipe.getId());
            psDeleteIngredients.executeUpdate();

            PreparedStatement psInsertIngredient = connection.prepareStatement(
                    "INSERT INTO refridgerate.recipeingredient (recipeid, ingredientid, quantityneeded) VALUES (?, ?, ?)"
            );

            for (SimplifiedIngredientLocal ingredient : existingRecipe.getIngredientUsed()) {
                psInsertIngredient.setInt(1, existingRecipe.getId());
                psInsertIngredient.setInt(2, ingredient.getId());
                psInsertIngredient.setInt(3, ingredient.getQuantity());
                psInsertIngredient.executeUpdate();
            }

            return GetRecipe(existingRecipe.getId());
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public boolean DeleteRecipe(int recipeId) {
        try (Connection connection = getConnected()) {
            PreparedStatement psDeleteIngredients = connection.prepareStatement(
                    "DELETE FROM refridgerate.recipeingredient WHERE recipeid = ?"
            );
            psDeleteIngredients.setInt(1, recipeId);
            psDeleteIngredients.executeUpdate();

            PreparedStatement psDeleteRecipe = connection.prepareStatement(
                    "DELETE FROM refridgerate.recipe WHERE recipeid = ?"
            );
            psDeleteRecipe.setInt(1, recipeId);

            int rowsAffected = psDeleteRecipe.executeUpdate();
            return rowsAffected > 0;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}