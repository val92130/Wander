package com.wander.game.models;

import com.badlogic.gdx.math.Vector2;

/**
 * Created by valentin on 08/12/2015.
 */
public class PlayerModel {
    @com.google.gson.annotations.SerializedName("Pseudo")
    public String Pseudo;
    @com.google.gson.annotations.SerializedName("Position")
    public WanderVector Position;
    @com.google.gson.annotations.SerializedName("Direction")
    public String Direction;
}
