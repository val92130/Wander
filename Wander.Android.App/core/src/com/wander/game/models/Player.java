package com.wander.game.models;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.badlogic.gdx.physics.box2d.Fixture;
import com.badlogic.gdx.physics.box2d.FixtureDef;
import com.badlogic.gdx.physics.box2d.PolygonShape;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.InputListener;
import com.wander.game.Constants;
import com.wander.game.GameMap;
import com.wander.game.MainGame;
import com.wander.game.util;

import java.util.Date;

/**
 * Created by valentin on 08/12/2015.
 */
public class Player {

    Sprite sprite;
    Vector2 position;
    String pseudo;
    GameMap map;
    EDirection direction;
    BitmapFont font;
    String textMessageContent;
    private Date messageTime, messageTimeEnd;

    public Player(GameMap map, String pseudo, Vector2 position, Sprite sprite)
    {
        this.sprite = sprite;
        this.sprite.setSize(15 * map.getScaleRatio(),25 * map.getScaleRatio() );
        this.position = position;
        this.pseudo = pseudo;
        this.map = map;
        this.direction = EDirection.Idle;
        this.textMessageContent = "";
        this.messageTime = new Date();
        this.messageTimeEnd = new Date();
    }

    public void render(SpriteBatch batch){

        sprite.draw(batch);
        if(this.font == null)
        {
            this.font = new BitmapFont();
            this.font.getData().setScale(2,2);
        }
        float fontWidth = util.GetStringWidth(font, this.pseudo);

        this.font.draw(batch, this.pseudo, sprite.getX() + this.sprite.getWidth() / 2 - fontWidth / 2, sprite.getY() + this.sprite.getHeight() + 35);

        if(this.textMessageContent != "")
        {
            float messageWidth = util.GetStringWidth(font, this.textMessageContent);
            this.font.setColor(0.75f,1,1,1);
            this.font.draw(batch, this.textMessageContent, sprite.getX() + this.sprite.getWidth() / 2 - messageWidth / 2, sprite.getY() + this.sprite.getHeight() + 85);
            this.font.setColor(1,1,1,1);
        }

    }

    public void update(float delta){
        sprite.setPosition(this.position.x * this.map.getScaleRatio(), this.position.y * this.map.getScaleRatio() - this.sprite.getHeight());
        if (this.textMessageContent != "") {
            this.messageTime = new Date();
            long nTime = (this.messageTime.getTime() - this.messageTimeEnd.getTime()) ;
            if (nTime >= 5000) {
                this.messageTimeEnd = this.messageTime;
                this.textMessageContent = "";
            }
        }
    }

    public void setTextMessage(String textMessage){
        if(textMessage != null)
        {
            this.textMessageContent = textMessage;
            this.messageTimeEnd = new Date();
            this.messageTime = messageTimeEnd;
        }
    }

    public String getPseudo(){
        return this.pseudo;
    }

    public Vector2 getPosition(){
        return this.position;
    }

    public EDirection getDirection() {
        return this.direction;
    }

    public void setPosition(Vector2 newPos){
        this.position = newPos;
    }

    public Sprite getSprite(){return this.sprite;}

}
