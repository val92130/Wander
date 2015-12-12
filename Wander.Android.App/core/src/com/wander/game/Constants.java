package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input;

/**
 * Created by val on 07/12/2015.
 */
public class Constants {
    public static int MAP_SIZE = 500;
    public static int CAMERA_SPEED = 3000;
    public static double CAMERA_ZOOM_SPEED = 0.2;
    // Keyboard input
    public static int KEY_MOVE_LEFT = Input.Keys.LEFT;
    public static int KEY_MOVE_RIGHT = Input.Keys.RIGHT;
    public static int KEY_MOVE_UP = Input.Keys.UP;
    public static int KEY_MOVE_DOWN = Input.Keys.DOWN;
    public static int KEY_TOGGLE_NIGHT = Input.Keys.N;

    // Tile Map
    public static int TILE_SIZE = 16;

    //UI
    private static float width = Gdx.graphics.getWidth();
    private static float height = Gdx.graphics.getHeight();
    private static float padding = height * 0.10f;
    private static float btnWidth = width * 0.8f;
    private static float btnHeight = height / 10;

    public static float BTN_MENU_WIDTH = btnWidth;
    public static float BTN_MENU_HEIGHT = btnHeight;
    public static float BTN_MENU_PADDING = padding;



}
