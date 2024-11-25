package com.example.serversideapp.shared;

import lombok.Getter;
import lombok.Setter;

@Getter @Setter
public class UserLocal {
    private int id;
    private String name;
    private String password;
    private String firstN;
    private String lastN;

    public UserLocal(int id, String name, String password, String firstN, String lastN) {
        this.id = id;
        this.name = name;
        this.password = password;
        this.firstN = firstN;
        this.lastN = lastN;
    }
}
