package com.example.serversideapp.back;

import com.example.serversideapp.shared.IngredientLocal;

import java.sql.*;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Date;

public class DBIngredientQuery implements DBIngredientManager {
    public DBIngredientQuery() {
    }

    @Override
    public ArrayList<IngredientLocal> GetAllIngredients() {
        //Goes through all ingredients in the database and adds them to the ans arraylist
        try (Connection connection = getConnected()) {
            PreparedStatement psIngredients = connection.prepareStatement("SELECT * FROM refridgerate.ingredient");
            PreparedStatement psBatches = connection.prepareStatement("SELECT sum(refridgerate.inventory.quantity) FROM refridgerate.inventory WHERE ingredientid = ? AND expirationdate > now()::date;");
            PreparedStatement psDates = connection.prepareStatement("SELECT refridgerate.inventory.quantity,refridgerate.inventory.expirationdate FROM refridgerate.inventory WHERE ingredientid = ? AND expirationdate > now()::date");
            return getListOfIngredient(psIngredients.executeQuery(), psBatches, psDates);
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    public IngredientLocal UpdateIngredient(int ingredientId, int quantity, int daysUntilBad) {
        Date todaysDate = new Date();
        try(Connection connection = getConnected()){
            PreparedStatement psUpdateIngredient = connection.prepareStatement("INSERT INTO refridgerate.inventory VALUES (DEFAULT, ?, ?, CAST(? AS refridgerate.action_type), ?, CAST(? AS DATE), CAST(? AS DATE));");
            psUpdateIngredient.setInt(1, ingredientId);//IngredientId
            psUpdateIngredient.setInt(2, 1);//chefId, for now stuck at 1
            psUpdateIngredient.setString(3, quantity > 0 ? "Add" : "Subtract");//ActionType, either Add or Subtract.
            psUpdateIngredient.setInt(4, quantity);//Quantity
            psUpdateIngredient.setString(5, todaysDate.toString());//Day of change
            psUpdateIngredient.setString(6, todaysDate.toInstant().plus(3, ChronoUnit.DAYS).toString());//Expiration date
            psUpdateIngredient.executeUpdate();
            //Done with updating, now get total again
            PreparedStatement psIngredients = connection.prepareStatement("SELECT * FROM refridgerate.ingredient WHERE ingredientid = ?");
            PreparedStatement psBatches = connection.prepareStatement("SELECT sum(refridgerate.inventory.quantity) FROM refridgerate.inventory WHERE ingredientid = ? AND expirationdate > now()::date;");
            PreparedStatement psDates = connection.prepareStatement("SELECT refridgerate.inventory.quantity,refridgerate.inventory.expirationdate FROM refridgerate.inventory WHERE ingredientid = ? AND expirationdate > now()::date");
            psIngredients.setInt(1, ingredientId);
            return getListOfIngredient(psIngredients.executeQuery(), psBatches, psDates).getFirst();
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
            psBatches.setInt(1, rsIngredients.getInt(1));
            psDates.setInt(1, rsIngredients.getInt(1));
            ResultSet rsBatches = psBatches.executeQuery();
            rsBatches.next(); //Read through all and count
            ans.add(new IngredientLocal(rsIngredients.getInt(1),
                    rsIngredients.getString(2),
                    rsIngredients.getFloat(3),
                    rsBatches.getInt(1),
                    findRecentDate(psDates.executeQuery())));
        }
        return ans;
    }

    private Connection getConnected() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres", "postgres", "1234");
    }
}
