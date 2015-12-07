package com.wander.game.models;

/**
 * Created by valentin on 07/12/2015.
 */
public class UserModel
{
    @com.google.gson.annotations.SerializedName("Login")
    public String Login;
    @com.google.gson.annotations.SerializedName("Email")
    public String Email;
    @com.google.gson.annotations.SerializedName("Password")
    public String Password;
    @com.google.gson.annotations.SerializedName("Sex")
    public int Sex;
}
