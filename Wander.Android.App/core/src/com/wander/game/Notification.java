package com.wander.game;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.Vector2;
import com.wander.game.MainGame;
import com.wander.game.models.EMessageType;
import com.wander.game.util;

/**
 * Created by valentin on 09/12/2015.
 */
public class Notification {

    private ShapeRenderer shapeRenderer;
    private Vector2 position;
    private EMessageType messageType;
    private String content;
    private MainGame game;
    private BitmapFont font;

    public Notification(MainGame game, Vector2 position, String content, EMessageType type)
    {
        this.game = game;
        this.position = position;
        this.content = content;
        this.messageType = type;
        this.position = position;
    }

    public Vector2 getPosition()
    {
        return this.position;
    }

    public void setPosition(Vector2 pos)
    {
        this.position = pos;
    }

    public float getWidth()
    {
        return util.GetStringWidth(this.font, this.content);
    }

    public float getHeight()
    {
        return util.GetStringHeight(this.font, this.content);
    }

    public String getContent()
    {
        return this.content;
    }

}
