package com.wander.game;

import com.badlogic.gdx.Gdx;

/**
 * Created by valentin on 07/12/2015.
 */
public class MainGame {

    HubService hubService;
    public MainGame()
    {
        hubService = new HubService("http://localhost:1906", "GameHub");
        hubService.start();
        hubService.connect();
    }

    public void render()
    {

    }

    public void update()
    {

    }
}
