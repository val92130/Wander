package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.InputProcessor;
import com.badlogic.gdx.math.Vector2;
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
            game.getCameraManager().move(new Vector2(-Constants.CAMERA_SPEED * Gdx.graphics.getDeltaTime(), 0));
        }

        if(Gdx.input.isKeyPressed(Constants.KEY_MOVE_RIGHT))
        {
            game.getCameraManager().move(new Vector2(Constants.CAMERA_SPEED * Gdx.graphics.getDeltaTime(), 0));
        }

        if(Gdx.input.isKeyPressed(Constants.KEY_MOVE_UP))
        {
            game.getCameraManager().move(new Vector2(0, Constants.CAMERA_SPEED * Gdx.graphics.getDeltaTime()));
        }

        if(Gdx.input.isKeyPressed(Constants.KEY_MOVE_DOWN))
        {
            game.getCameraManager().move(new Vector2(0, -Constants.CAMERA_SPEED * Gdx.graphics.getDeltaTime()));
        }

    }

    public boolean keyDown(int keycode) {
        if(keycode == Constants.KEY_TOGGLE_NIGHT)
        {
            //game.getAmbientEventManager().setNightTime(!game.getAmbientEventManager().getNightTime());
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
