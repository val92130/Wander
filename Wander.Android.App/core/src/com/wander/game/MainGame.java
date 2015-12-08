package com.wander.game;

import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.wander.game.models.NotificationMessage;
import com.wander.game.models.PlayerModel;
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
    private Sprite playerSprite;
    private GameScreen gameScreen;

    public MainGame()
    {
        hubService = new HubService("http://localhost:1906", "GameHub");
        loginScreen = new LoginScreen(this);
    }

    @Override
    public void create() {
        hubService.start();
        this.setScreen(loginScreen);

        Texture t = new Texture(Gdx.files.internal("images/player.png"));
        this.playerSprite = new Sprite(t);

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
            gameScreen = new GameScreen(this);
            this.setScreen(this.gameScreen);
        }
    }

    public GameScreen getGameScreen()
    {
        return this.gameScreen;
    }

    public HubService getHubService()
    {
        return hubService;
    }

    public String getUserPseudo(){
        return userPseudo;
    }

    public Sprite getPlayerSprite(){
        return this.playerSprite;
    }


}
