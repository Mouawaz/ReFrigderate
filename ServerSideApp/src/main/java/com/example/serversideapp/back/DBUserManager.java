package com.example.serversideapp.back;

import com.example.serversideapp.shared.UserLocal;

public interface DBUserManager {
    UserLocal getUserByName(String username);
    
}
