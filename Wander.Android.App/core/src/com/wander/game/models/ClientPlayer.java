package com.wander.game.models;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.wander.game.Constants;
import com.wander.game.GameMap;
import com.wander.game.MainGame;

import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.ExecutionException;

/**
 * Created by val on 08/12/2015.
 */
public class ClientPlayer extends Player {
    public static int speed = 70;
    private Timer updateTimer;
    private int updateTimeMs = 55;
    private boolean moved = false;

    public ClientPlayer(GameMap map, String pseudo, Vector2 position, Sprite sprite) {
        super(map, pseudo, position, sprite);
        this.position = new Vector2(position.x, Constants.TILE_SIZE * Constants.MAP_SIZE - position.y );
        this.updateTimer = new Timer();

        this.updateTimer.scheduleAtFixedRate(new TimerTask() {
            @Override
            public void run() {
                updatePositionServer();
            }
        }, updateTimeMs, updateTimeMs);


    }

    @Override
    public void update(float delta)
    {
        super.update(delta);
    }

    @Override
    public void render(SpriteBatch batch)
    {
        super.render(batch);
    }

    public void move(EDirection direction)
    {
        switch (direction) {
            case Left:
                this.position.x -= speed * Gdx.graphics.getDeltaTime();
                break;
            case Right:
                this.position.x += speed * Gdx.graphics.getDeltaTime();
                break;
            case Up:
                this.position.y += speed * Gdx.graphics.getDeltaTime();
                break;
            case Down:
                this.position.y -= speed * Gdx.graphics.getDeltaTime();
                break;
        }
        this.moved = true;
    }

    public void moveOffset(Vector2 offset)
    {
        this.position.x += offset.x;
        this.position.y += offset.y;

        this.moved = true;
    }

    private void updatePositionServer()
    {
        if(this.moved)
        {
            WanderVector pos = new WanderVector();
            pos.X = this.position.x;
            pos.Y = -(this.position.y - Constants.TILE_SIZE * Constants.MAP_SIZE);

            try {
                this.map.getGameScreen().getMainGame().getHubService().getHub().invoke("UpdatePosition", pos, this.direction).get();
            } catch (InterruptedException e) {
                e.printStackTrace();
            } catch (ExecutionException e) {
                e.printStackTrace();
            }
            this.moved = !this.moved;
        }

    }
}
