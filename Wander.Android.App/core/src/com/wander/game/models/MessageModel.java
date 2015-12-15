package com.wander.game.models;

/**
 * Created by valentin on 11/12/2015.
 */
public class MessageModel {
    @com.google.gson.annotations.SerializedName("Content")
    public String Content;
    @com.google.gson.annotations.SerializedName("UserName")
    public String UserName;
    @com.google.gson.annotations.SerializedName("Sex")
    public int Sex;
    @com.google.gson.annotations.SerializedName("UserId")
    public int UserId;
    @com.google.gson.annotations.SerializedName("Hour")
    public String Hour;
}
