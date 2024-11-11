package com.example.serversideapp.back;

import com.example.serversideapp.shared.IngredientLocal;

import java.sql.*;
import java.util.ArrayList;

public class DBQuery implements DBManager{
    public DBQuery(){}
    @Override
    public ArrayList<IngredientLocal> GetAllIngredients() {
        ArrayList<IngredientLocal> ans = new ArrayList<>();
        //Goes through all ingredients in the database and adds them to the ans arraylist
        try(Connection connection = getConnected()){
            PreparedStatement psIngredients = connection.prepareStatement("select * from refrigderate.ingredient");
            ResultSet rsIngredients = psIngredients.executeQuery();
            while (rsIngredients.next()){
                ans.add(new IngredientLocal(rsIngredients.getInt(1),
                        rsIngredients.getString(2),
                        rsIngredients.getFloat(3),
                        rsIngredients.getInt(4),
                        rsIngredients.getDate(5)));
            }
            return ans;
        }
        catch (SQLException e){
            throw new RuntimeException(e);
        }
    }
    private Connection getConnected() throws SQLException {
        return DriverManager.getConnection(
                "jdbc:postgresql://localhost:5432/postgres", "postgres", "1234");
    }
}
