package com.example.serversideapp.back;

import com.example.serversideapp.shared.IngredientLocal;

import java.sql.*;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Date;

public class DBIngredientQuery extends DBGeneral implements DBIngredientManager {
    public DBIngredientQuery() {
    }

    @Override
    public ArrayList<IngredientLocal> GetAllIngredients() {
        //Goes through all ingredients in the database and adds them to the ans arraylist
        try (Connection connection = getConnected()) {
            PreparedStatement psIngredients = connection.prepareStatement("SELECT * FROM refridgerate.ingredient");
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
            PreparedStatement psUpdateIngredient = connection.prepareStatement("INSERT INTO refridgerate.inventory VALUES (1, DEFAULT, ?, ?, CAST(? AS refridgerate.action_type), ?, CAST(? AS DATE), CAST(? AS DATE));");
            psUpdateIngredient.setInt(1, ingredientId);//IngredientId
            psUpdateIngredient.setInt(2, 1);//chefId, for now stuck at 1
            psUpdateIngredient.setString(3, quantity > 0 ? "Add" : "Subtract");//ActionType, either Add or Subtract.
            psUpdateIngredient.setInt(4, quantity);//Quantity
            psUpdateIngredient.setString(5, todaysDate.toString());//Day of change
            psUpdateIngredient.setString(6, todaysDate.toInstant().plus(daysUntilBad, ChronoUnit.DAYS).toString());//Expiration date
            psUpdateIngredient.executeUpdate();
            //Done with updating, now get total again
            PreparedStatement psIngredients = connection.prepareStatement("SELECT * FROM refridgerate.ingredient WHERE ingredientid = ?");
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
        try(Connection connection = getConnected()){
            PreparedStatement psUpdateWarnings = connection.prepareStatement("update refridgerate.ingredient as i set yellowdays = c.yellowDays, yellowamount = c.yellowAmount, reddays = c.redDays, redamount = c.redAmount from (values (?, ?, ?, ?)) as c(yellowDays, yellowAmount, redDays, redAmount) where i.ingredientid = ?;");
            psUpdateWarnings.setInt(1, yellowTime);
            psUpdateWarnings.setInt(2, yellowAmount);
            psUpdateWarnings.setInt(3, redTime);
            psUpdateWarnings.setInt(4, redAmount);
            psUpdateWarnings.setInt(5, ingredientId);
            int amountChanged = psUpdateWarnings.executeUpdate();
            return amountChanged != 0;
        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }

    private Date findRecentDate(ResultSet rsDates) throws SQLException {
        rsDates.next();
        Date minDate = rsDates.getDate(2);
        int count = rsDates.getInt(1);
        while (rsDates.next()) {
            if (count <= 0) {
                minDate = rsDates.getDate(2);
            }
            count += rsDates.getInt(1);
        }
        return minDate;
    }

    private ArrayList<IngredientLocal> getListOfIngredient(ResultSet rsIngredients, PreparedStatement psBatches, PreparedStatement psDates) throws SQLException {
        ArrayList<IngredientLocal> ans = new ArrayList<>();

        while (rsIngredients.next()) {
            try {
                int ingredientID = rsIngredients.getInt(2);
                String name = rsIngredients.getString(3);
                float cost = rsIngredients.getFloat(4);
                psBatches.setInt(1, ingredientID);
                psDates.setInt(1, ingredientID);
                ResultSet rsBatches = psBatches.executeQuery();
                int batchSum = rsBatches.next() ? rsBatches.getInt(1) : 0;
                ResultSet rsDates = psDates.executeQuery();
                Date recentDate = rsDates.next() ? findRecentDate(rsDates) : null;

                ans.add(new IngredientLocal(ingredientID, name, cost, batchSum, recentDate,
                        rsIngredients.getInt(5),
                        rsIngredients.getInt(6),
                        rsIngredients.getInt(7),
                        rsIngredients.getInt(8)));
            } catch (SQLException e) {
                System.err.println("Error processing ingredient: " + e.getMessage());
            }
        }

        return ans;
    }


}
