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
    public static int speed = 82;
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
        this.direction = EDirection.Idle;
    }

    @Override
    public void render(SpriteBatch batch)
    {
        super.render(batch);
    }

    public void move(EDirection direction)
    {
        Vector2 newPos = new Vector2(this.position.x, this.position.y);
        switch (direction) {
            case Left:
                newPos.x -= speed * Gdx.graphics.getDeltaTime();
                break;
            case Right:
                newPos.x += speed * Gdx.graphics.getDeltaTime();
                break;
            case Up:
                newPos.y += speed * Gdx.graphics.getDeltaTime();
                break;
            case Down:
                newPos.y -= speed * Gdx.graphics.getDeltaTime();
                break;
        }
        int tileX = (int)(newPos.x / Constants.TILE_SIZE) + 1;
        int tileY = (int)(newPos.y / Constants.TILE_SIZE) - 1;

        if(!map.isCollision(tileX, tileY))
        {
            this.position = newPos;
            this.moved = true;
            this.direction = direction;
        }

    }

    public void moveOffset(Vector2 offset)
    {
        Vector2 newPos = new Vector2(this.position.x, this.position.y);

        newPos.x += offset.x;
        newPos.y += offset.y;

        int tileX = (int)(newPos.x / Constants.TILE_SIZE) + 1;
        int tileY = (int)(newPos.y / Constants.TILE_SIZE) - 1;


        if(!map.isCollision(tileX, tileY))
        {
            if(offset.x > 0 && offset.x > offset.y)
            {
                this.direction = EDirection.Right;
            }else if(offset.x < 0 && offset.y > offset.x)
            {
                this.direction = EDirection.Left;
            } else if(offset.y > 0)
            {
                this.direction = EDirection.Up;
            } else if(offset.y < 0)
            {
                this.direction = EDirection.Down;
            }else{
                this.direction = EDirection.Idle;
            }

            this.position = newPos;
            this.moved = true;
        }

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
