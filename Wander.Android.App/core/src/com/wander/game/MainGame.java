package com.wander.game;

import com.badlogic.gdx.Game;
import com.wander.game.models.NotificationMessage;
import com.wander.game.screens.GameScreen;
import com.wander.game.screens.LoginScreen;
import com.wander.game.screens.MainMenuScreen;
import com.wander.game.services.HubService;

import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;

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
        this.setScreen(loginScreen);

        hubService.getHub().on("notify", new SubscriptionHandler1<NotificationMessage>() {

            @Override
            public void run(NotificationMessage o) {
                System.out.println("NOTIFICATION FROM SERVER" + " : " + o.Content );
            }

        }, NotificationMessage.class);

    }

    @Override
    public void render() {
        this.update();
        super.render();
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

    public void startGameScreen()
    {
        if(this.connected && this.userPseudo != null)
        {
            this.setScreen(new GameScreen(this));
        }
    }

    public HubService getHubService()
    {
        return hubService;
    }

    public String getUserPseudo(){
        return userPseudo;
    }


}
