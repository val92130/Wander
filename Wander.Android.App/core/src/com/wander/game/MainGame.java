package com.wander.game;

import com.badlogic.gdx.Game;
import com.wander.game.screens.LoginScreen;
import com.wander.game.screens.MainMenuScreen;
import com.wander.game.services.HubService;

/**
 * Created by valentin on 07/12/2015.
 */
public class MainGame extends Game {

    private HubService hubService;
    private LoginScreen loginScreen;
    private MainMenuScreen mainMenuScreen;
    private boolean connected;
    private String userPseudo;

    public MainGame()
    {
        hubService = new HubService("http://localhost:1906", "GameHub");
        loginScreen = new LoginScreen(this);
    }

    @Override
    public void create() {
        hubService.start();
        System.out.println(hubService.connect("val_92_", "1234"));
        this.setScreen(loginScreen);
    }

    @Override
    public void render()
    {
        super.render();
        this.update();
    }

    public void update()
    {

    }

    public void SetConnected(String pseudo)
    {
        this.connected = true;
        this.userPseudo = pseudo;
        mainMenuScreen = new MainMenuScreen(this);
        this.setScreen(mainMenuScreen);
    }

    public HubService getHubService()
    {
        return hubService;
    }
}
