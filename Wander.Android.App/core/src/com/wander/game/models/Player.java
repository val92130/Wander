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

/**
 * Created by valentin on 08/12/2015.
 */
public class Player {

    Sprite sprite;
    Vector2 position;
    String pseudo;
    GameMap map;
    String direction;
    BitmapFont font;
    Body body;

    public Player(GameMap map, String pseudo, Vector2 position, Sprite sprite)
    {
        this.sprite = sprite;
        this.sprite.setSize(15 * map.getScaleRatio(),25 * map.getScaleRatio() );
        this.position = position;
        this.pseudo = pseudo;
        this.map = map;
        this.direction = "7";

        BodyDef bodyDef = new BodyDef();
        bodyDef.type = BodyDef.BodyType.DynamicBody;
        // We are going to use 1 to 1 dimensions.  Meaning 1 in physics engine
        // Set our body to the same position as our sprite
        bodyDef.position.set(100, (500 * 16 * 4 )- 200 );

        // Create a body in the world using our definition
        body = map.getWorld().createBody(bodyDef);

        // Now define the dimensions of the physics shape
        PolygonShape shape = new PolygonShape();
        // We are a box, so this makes sense, no?
        // Basically set the physics polygon to a box with the same dimensions
        shape.setAsBox(sprite.getWidth()/2, sprite.getHeight()/2);

        // FixtureDef is a confusing expression for physical properties
        // you also define it's properties like density, restitution and others
        // If you are wondering, density and area are used to calculate over all
        FixtureDef fixtureDef = new FixtureDef();
        fixtureDef.shape = shape;
        fixtureDef.density = 1f;

        Fixture fixture = body.createFixture(fixtureDef);

        // Shape is the only disposable of the lot, so get rid of it
        shape.dispose();
    }

    public void render(SpriteBatch batch){

        sprite.draw(batch);
        if(this.font == null)
        {
            this.font = new BitmapFont();
            this.font.getData().setScale(2,2);
        }
        float fontWidth = util.GetStringWidth(font, this.pseudo);

        this.font.draw(batch, this.pseudo, sprite.getX() + this.sprite.getWidth()/2 - fontWidth/2, sprite.getY() + this.sprite.getHeight() + 35);
    }

    public void update(float delta){
        sprite.setPosition(this.position.x * this.map.getScaleRatio(), this.position.y * this.map.getScaleRatio() - this.sprite.getHeight() );
        body.setTransform(sprite.getX() + sprite.getWidth()/2, sprite.getY() + sprite.getHeight()/2, body.getAngle());
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
