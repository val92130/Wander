package com.wander.game.player;

import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.wander.game.Constants;
import com.wander.game.GameMap;

/**
 * Created by valentin on 08/12/2015.
 */
public class ServerPlayer extends Player {

    Vector2 newPosition;
    public ServerPlayer(GameMap map, String pseudo, Vector2 position, Sprite sprite) {
        super(map, pseudo, position, sprite);
        this.position = new Vector2(position.x, Constants.TILE_SIZE * Constants.MAP_SIZE - position.y );
        this.newPosition = this.position;
    }

    @Override
    public void update(float delta)
    {
        super.update(delta);
        this.position = this.position.lerp(new Vector2(this.newPosition.x, this.newPosition.y),0.1f);
    }

    @Override
    public void render(SpriteBatch batch)
    {
        super.render(batch);
    }

    public void updateInfos(Vector2 newPosition, String newDirection)
    {
        Vector2 correctPos = new Vector2(newPosition.x, Constants.TILE_SIZE * Constants.MAP_SIZE - newPosition.y );
        this.newPosition = correctPos;
    }
}
