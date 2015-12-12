package com.wander.game.models;

/**
 * Created by val on 12/12/2015.
 */
public class ServerPropertyModel {
    @com.google.gson.annotations.SerializedName("PropertyId")
    public int PropertyId;
    @com.google.gson.annotations.SerializedName("PropertyName")
    public String PropertyName;
    @com.google.gson.annotations.SerializedName("PropertyDescription")
    public String PropertyDescription;
    @com.google.gson.annotations.SerializedName("Threshold")
    public int Threshold;
    @com.google.gson.annotations.SerializedName("Price")
    public int Price;
}
