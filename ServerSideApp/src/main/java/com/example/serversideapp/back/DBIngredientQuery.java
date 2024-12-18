package com.example.serversideapp.back;

import com.example.serversideapp.shared.IngredientLocal;
import com.example.serversideapp.shared.IngredientLocal.IngredientCategory;
import org.hibernate.mapping.List;

import java.sql.*;
import java.time.temporal.ChronoUnit;
import java.util.*;
import java.util.Date;

public class DBIngredientQuery extends DBGeneral implements DBIngredientManager {
    public DBIngredientQuery() {
    }

    @Override
    public ArrayList<IngredientLocal> GetAllIngredients() {
        try (Connection connection = getConnected()) {
            PreparedStatement psIngredients = connection.prepareStatement("SELECT * FROM refridgerate.ingredient ORDER BY ingredientid");
            PreparedStatement psBatches = connection.prepareStatement("SELECT sum(refridgerate.inventory.quantity) FROM refridgerate.inventory WHERE ingredientid = ?;");
            PreparedStatement psDates = connection.prepareStatement("SELECT refridgerate.inventory.quantity,refridgerate.inventory.expirationdate FROM refridgerate.inventory WHERE ingredientid = ?;");
            return getListOfIngredient(psIngredients.executeQuery(), psBatches, psDates);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public IngredientLocal UpdateIngredient(int ingredientId, int quantity, int daysUntilBad) {
        Date todaysDate = new Date();
        try (Connection connection = getConnected()) {
            PreparedStatement psUpdateIngredient = connection.prepareStatement(
                    "INSERT INTO refridgerate.inventory (fridgeid, inventoryid, ingredientid, chefid, actiontype, quantity, date, expirationdate) " +
                            "VALUES (1, DEFAULT, ?, ?, CAST(? AS refridgerate.action_type), ?, CAST(? AS DATE), CAST(? AS DATE));"
            );
            psUpdateIngredient.setInt(1, ingredientId);
            psUpdateIngredient.setInt(2, 1); //chefId, for now stuck at 1
            psUpdateIngredient.setString(3, quantity > 0 ? "Add" : "Subtract");
            psUpdateIngredient.setInt(4, quantity);
            psUpdateIngredient.setString(5, todaysDate.toString());
            psUpdateIngredient.setString(6, todaysDate.toInstant().plus(daysUntilBad, ChronoUnit.DAYS).toString());
            psUpdateIngredient.executeUpdate();
            //Done with updating, now get total again
            PreparedStatement psIngredients = connection.prepareStatement("SELECT * FROM refridgerate.ingredient WHERE ingredientid = ? ORDER BY ingredientid");
            PreparedStatement psBatches = connection.prepareStatement("SELECT sum(refridgerate.inventory.quantity) FROM refridgerate.inventory WHERE ingredientid = ?;");
            PreparedStatement psDates = connection.prepareStatement("SELECT refridgerate.inventory.quantity,refridgerate.inventory.expirationdate FROM refridgerate.inventory WHERE ingredientid = ?;");
            psIngredients.setInt(1, ingredientId);
            return getListOfIngredient(psIngredients.executeQuery(), psBatches, psDates).getFirst();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public Boolean UpdateWarningAmount(int ingredientId, int yellowAmount, int redAmount, int yellowTime, int redTime) {
        try (Connection connection = getConnected()) {
            PreparedStatement psUpdateWarnings = connection.prepareStatement(
                    "UPDATE refridgerate.ingredient AS i " +
                            "SET yellowdays = c.yellowDays, yellowamount = c.yellowAmount, reddays = c.redDays, redamount = c.redAmount " +
                            "FROM (VALUES (?, ?, ?, ?)) AS c(yellowDays, yellowAmount, redDays, redAmount) " +
                            "WHERE i.ingredientid = ?;"
            );
            psUpdateWarnings.setInt(1, yellowTime);
            psUpdateWarnings.setInt(2, yellowAmount);
            psUpdateWarnings.setInt(3, redTime);
            psUpdateWarnings.setInt(4, redAmount);
            psUpdateWarnings.setInt(5, ingredientId);
            int amountChanged = psUpdateWarnings.executeUpdate();
            return amountChanged != 0;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public IngredientLocal CreateIngredient(String name, String CATEGORY, float cost) {
        try(Connection connection = getConnected()){
            PreparedStatement psQuickId = connection.prepareStatement("SELECT max(ingredientid) FROM refridgerate.ingredient");
            PreparedStatement psInsertIngredient = connection.prepareStatement("INSERT INTO refridgerate.ingredient VALUES (null, ?, ?, CAST(? AS refridgerate.ingredient_category), ?)");
            ResultSet rsQuickId = psQuickId.executeQuery();
            psInsertIngredient.setInt(1, rsQuickId.getInt(1));
            psInsertIngredient.setString(2, name);
            psInsertIngredient.setString(3, CATEGORY);
            psInsertIngredient.setFloat(4, cost);
            psInsertIngredient.executeUpdate();
            return GetAllIngredients().getLast();
        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }

    private Date findRecentDate(ResultSet rsDates) throws SQLException {
        ArrayList<ArrayList<Object>> DateCount2D = new ArrayList<>();
        while (rsDates.next()){
            int mod = rsDates.getInt(1);
            if (mod > 0){
                DateCount2D.add(new ArrayList<>(Arrays.asList(rsDates.getDate(2), mod)));
            }
            else{
                for (int i = 0; i < DateCount2D.size(); i++) {
                    int value = Integer.valueOf(DateCount2D.get(i).get(1).toString());
                    if (value > -mod){
                        DateCount2D.get(i).set(1, value+mod);
                        break;
                    }
                    mod -= value;
                    DateCount2D.remove(DateCount2D.get(i));
                    i--;
                }
            }
        }
        if (DateCount2D.isEmpty()){
            return new Date();
        }
        return (Date)DateCount2D.getFirst().getFirst();
    }

    private ArrayList<IngredientLocal> getListOfIngredient(ResultSet rsIngredients, PreparedStatement psBatches, PreparedStatement psDates) throws SQLException {
        ArrayList<IngredientLocal> ans = new ArrayList<>();

        while (rsIngredients.next()) {
            try {
                int ingredientID = rsIngredients.getInt(2);
                String name = rsIngredients.getString(3);
                IngredientCategory category = IngredientCategory.valueOf(rsIngredients.getString(4).toUpperCase());
                float cost = rsIngredients.getFloat(5);
                psBatches.setInt(1, ingredientID);
                psDates.setInt(1, ingredientID);
                ResultSet rsBatches = psBatches.executeQuery();
                int batchSum = rsBatches.next() ? rsBatches.getInt(1) : 0;
                ResultSet rsDates = psDates.executeQuery();
                Date recentDate = findRecentDate(rsDates);
                ans.add(new IngredientLocal(
                        ingredientID,
                        name,
                        cost,
                        batchSum,
                        recentDate,
                        rsIngredients.getInt(6),  // yellowAmount
                        rsIngredients.getInt(7),  // redAmount
                        rsIngredients.getInt(8),  // yellowDays
                        rsIngredients.getInt(9),  // redDays
                        category
                ));
            } catch (SQLException e) {
                System.err.println("Error processing ingredient: " + e.getMessage());
            }
        }

        return ans;
    }

    // New method to get ingredients by category
    public ArrayList<IngredientLocal> GetIngredientsByCategory(IngredientCategory category) {
        try (Connection connection = getConnected()) {
            PreparedStatement psIngredients = connection.prepareStatement(
                    "SELECT * FROM refridgerate.ingredient WHERE category = CAST(? AS refridgerate.ingredient_category)"
            );
            psIngredients.setString(1, category.name());

            PreparedStatement psBatches = connection.prepareStatement(
                    "SELECT sum(refridgerate.inventory.quantity) FROM refridgerate.inventory WHERE ingredientid = ?;"
            );
            PreparedStatement psDates = connection.prepareStatement(
                    "SELECT refridgerate.inventory.quantity,refridgerate.inventory.expirationdate FROM refridgerate.inventory WHERE ingredientid = ?;"
            );

            return getListOfIngredient(psIngredients.executeQuery(), psBatches, psDates);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}