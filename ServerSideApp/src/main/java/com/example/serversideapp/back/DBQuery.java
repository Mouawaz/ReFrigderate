package com.example.serversideapp.back;

import com.example.serversideapp.shared.IngredientLocal;

import java.sql.*;
import java.util.ArrayList;
import java.util.Date;

public class DBQuery implements DBManager {
    public DBQuery() {
    }

    @Override
    public ArrayList<IngredientLocal> GetAllIngredients() {
        ArrayList<IngredientLocal> ans = new ArrayList<>();
        //Goes through all ingredients in the database and adds them to the ans arraylist
        try (Connection connection = getConnected()) {
            PreparedStatement psIngredients = connection.prepareStatement("SELECT * FROM refridgerate.ingredient");
            PreparedStatement psBatches = connection.prepareStatement("SELECT sum(refridgerate.inventory.quantity) FROM refridgerate.inventory WHERE ingredientid = ? AND expirationdate > now()::date;");
            PreparedStatement psDates = connection.prepareStatement("SELECT refridgerate.inventory.quantity,refridgerate.inventory.expirationdate FROM refridgerate.inventory WHERE ingredientid = ? AND expirationdate > now()::date");
            ResultSet rsIngredients = psIngredients.executeQuery();
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
        } catch (SQLException e) {
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

    private Connection getConnected() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres", "postgres", "1234");
    }
}
