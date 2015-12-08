package com.wander.game.models;

import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.InputListener;
import com.wander.game.Constants;
import com.wander.game.GameMap;
import com.wander.game.MainGame;

/**
 * Created by valentin on 08/12/2015.
 */
public class Player {

    Sprite sprite;
    Vector2 position;
    String pseudo;
    GameMap map;
    String direction;

    public Player(GameMap map, String pseudo, Vector2 position, Sprite sprite)
    {
        this.sprite = sprite;
        this.sprite.setSize(15 * map.getScaleRatio(),25 * map.getScaleRatio() );
        this.position = position;
        this.pseudo = pseudo;
        this.map = map;
        this.direction = "7";
    }

    public void render(SpriteBatch batch){

        sprite.draw(batch);
    }

    public void update(float delta){
        sprite.setPosition(this.position.x * this.map.getScaleRatio(), this.position.y * this.map.getScaleRatio() - this.sprite.getHeight() );
    }

    public String getPseudo(){
        return this.pseudo;
    }

    public Vector2 getPosition(){
        return this.position;
    }

    public String getDirection() {
        return this.direction;
    }

    public void setPosition(Vector2 newPos){
        this.position = newPos;
    }

    public Sprite getSprite(){return this.sprite;}

}
