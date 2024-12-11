package com.example.serversideapp.shared;

import lombok.Getter;
import lombok.Setter;

@Getter @Setter
public class UserLocal {
    private int id;
    private String email;
    private String password;
    private String firstN;
    private String lastN;
    private int permissions;

    public UserLocal(int id, String email, String password, String firstN, String lastN, int permissions) {
        this.id = id;
        this.email = email;
        this.password = password;
        this.firstN = firstN;
        this.lastN = lastN;
        this.permissions = permissions;
    }
}
