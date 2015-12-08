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
import com.wander.game.MainGame;

/**
 * Created by valentin on 08/12/2015.
 */
public class Player {

    Sprite sprite;
    Vector2 position;
    String pseudo;
    MainGame game;
    String direction;

    public Player(MainGame game, String pseudo, Vector2 position, Sprite sprite)
    {
        this.sprite = sprite;
        this.sprite.setSize(this.sprite.getWidth() * 2,this.sprite.getHeight() * 2 );
        this.position = position;
        this.pseudo = pseudo;
        this.game = game;
        this.direction = "7";
    }

    public void render(SpriteBatch batch){

        sprite.draw(batch);
    }

    public void update(float delta){
        sprite.setPosition(this.position.x, this.position.y + (Constants.MAP_SIZE * Constants.TILE_SIZE )* game.getGameScreen().getMap().getScaleRatio() - this.sprite.getHeight());
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

}
