package com.wander.game.InputHandling;

import com.badlogic.gdx.input.GestureDetector;
import com.badlogic.gdx.math.Vector2;
import com.wander.game.Constants;
import com.wander.game.screens.GameScreen;

/**
 * Created by valentin on 09/12/2015.
 */
public class TouchInputManager implements GestureDetector.GestureListener {
    private GameScreen game;

    public TouchInputManager(GameScreen screen) {
        this.game = screen;
    }

    @Override
    public boolean touchDown(float x, float y, int pointer, int button) {
        return false;
    }

    @Override
    public boolean tap(float x, float y, int count, int button) {


        return false;
    }

    @Override
    public boolean longPress(float x, float y) {
        return false;
    }

    @Override
    public boolean fling(float velocityX, float velocityY, int button) {
        return false;
    }

    @Override
    public boolean pan(float x, float y, float deltaX, float deltaY) {
        this.game.getCameraManager().move(new Vector2(-deltaX, deltaY));
        return false;
    }

    @Override
    public boolean panStop(float x, float y, int pointer, int button) {
        return false;
    }

    @Override
    public boolean zoom(float initialDistance, float distance) {
        if (distance > initialDistance) {
            game.getCameraManager().zoom(-(0.01 * (distance / initialDistance)));
        } else {
            game.getCameraManager().zoom(0.01 * (initialDistance / distance));
        }
        return false;
    }

    @Override
    public boolean pinch(Vector2 initialPointer1, Vector2 initialPointer2, Vector2 pointer1, Vector2 pointer2) {
        return false;
    }
}
