package com.wander.game.models;

import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.wander.game.MainGame;

/**
 * Created by valentin on 08/12/2015.
 */
public class ServerPlayer extends Player {

    Vector2 newPosition;
    public ServerPlayer(MainGame game, String pseudo, Vector2 position, Sprite sprite) {
        super(game, pseudo, position, sprite);
        this.newPosition = position;
    }

    @Override
    public void update(float delta)
    {
        super.update(delta);
        this.position = this.position.lerp(new Vector2(this.newPosition.x, this.newPosition.y),1.35f);
    }

    @Override
    public void render(SpriteBatch batch)
    {
        super.render(batch);
    }

    public void updateInfos(Vector2 newPosition, String newDirection)
    {
        this.newPosition = newPosition;
    }
}
