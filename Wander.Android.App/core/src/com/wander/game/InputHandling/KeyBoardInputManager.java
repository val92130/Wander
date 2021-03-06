package com.wander.game.InputHandling;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.InputProcessor;
import com.badlogic.gdx.math.Vector2;
import com.wander.game.Constants;
import com.wander.game.models.EDirection;
import com.wander.game.screens.GameScreen;

/**
 * Created by val on 07/12/2015.
 */
public class KeyBoardInputManager implements InputProcessor {
    GameScreen game;
    public KeyBoardInputManager(GameScreen game)
    {
        this.game = game;
    }

    public void update()
    {
        if(Gdx.input.isKeyPressed(Constants.KEY_MOVE_LEFT))
        {
            game.getMap().getCurrentPlayer().move(EDirection.Left);
            //game.getCameraManager().move(new Vector2(-Constants.CAMERA_SPEED * Gdx.graphics.getDeltaTime(), 0));
        }

        if(Gdx.input.isKeyPressed(Constants.KEY_MOVE_RIGHT))
        {
            game.getMap().getCurrentPlayer().move(EDirection.Right);
            //game.getCameraManager().move(new Vector2(Constants.CAMERA_SPEED * Gdx.graphics.getDeltaTime(), 0));
        }

        if(Gdx.input.isKeyPressed(Constants.KEY_MOVE_UP))
        {
            game.getMap().getCurrentPlayer().move(EDirection.Up);
            //game.getCameraManager().move(new Vector2(0, Constants.CAMERA_SPEED * Gdx.graphics.getDeltaTime()));
        }

        if(Gdx.input.isKeyPressed(Constants.KEY_MOVE_DOWN))
        {
            game.getMap().getCurrentPlayer().move(EDirection.Down);
            //game.getCameraManager().move(new Vector2(0, -Constants.CAMERA_SPEED * Gdx.graphics.getDeltaTime()));
        }

    }

    public boolean keyDown(int keycode) {
        if(keycode == Input.Keys.E)
        {
            this.game.getMap().actionPressed();
        }
        return false;
    }

    public boolean keyUp(int keycode) {
        return false;
    }

    public boolean keyTyped(char character) {
        return false;
    }

    public boolean touchDown(int screenX, int screenY, int pointer, int button) {
        return false;
    }

    public boolean touchUp(int screenX, int screenY, int pointer, int button) {
        return false;
    }

    public boolean touchDragged(int screenX, int screenY, int pointer) {
        return false;
    }

    public boolean mouseMoved(int screenX, int screenY) {
        return false;
    }

    public boolean scrolled(int amount) {

        if(amount == -1)
        {
            game.getCameraManager().zoom(-Constants.CAMERA_ZOOM_SPEED);
        } else
        {
            game.getCameraManager().zoom(Constants.CAMERA_ZOOM_SPEED);
        }
        return false;
    }
}
