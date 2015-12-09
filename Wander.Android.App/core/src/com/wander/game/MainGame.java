package com.wander.game;

import com.badlogic.gdx.Application;
import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.wander.game.models.EMessageType;
import com.wander.game.models.NotificationMessage;
import com.wander.game.models.PlayerModel;
import com.wander.game.screens.GameScreen;
import com.wander.game.screens.LoginScreen;
import com.wander.game.screens.MainMenuScreen;
import com.wander.game.services.AndroidHubService;
import com.wander.game.services.HubService;
import com.wander.game.services.IHubService;

import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;

/**
 * Created by valentin on 07/12/2015.
 */
public class MainGame extends Game {

    private IHubService hubService;
    private LoginScreen loginScreen;
    private MainMenuScreen mainMenuScreen;
    private boolean connected;
    private String userPseudo;
    private GameScreen gameScreen;
    private Texture playerTexture = new Texture(Gdx.files.internal("images/player.png"));
    private NotificationManager notificationManager;
    private SpriteBatch uiSpritebatch;

    public MainGame()
    {
        this.notificationManager = new NotificationManager(this);

    }

    @Override
    public void create() {
        if(Gdx.app.getType() == Application.ApplicationType.Android) {
            this.hubService = new AndroidHubService("http://wander.nightlydev.fr", "GameHub");
        } else
        {
            this.hubService = new HubService("http://wander.nightlydev.fr", "GameHub");
        }
        loginScreen = new LoginScreen(this);


        hubService.start();
        this.setScreen(loginScreen);
        uiSpritebatch = new SpriteBatch();

        this.getHubService().getHub().on("notify", new SubscriptionHandler1<NotificationMessage>() {

            @Override
            public void run(NotificationMessage o) {
                addNotification(o.Content, EMessageType.valueOf(o.MessageType));
                System.out.println("NOTIFICATION FROM SERVER" + " : " + o.Content);
            }

        }, NotificationMessage.class);


    }

    @Override
    public void render() {
        this.update();
        super.render();

        uiSpritebatch.begin();
        this.notificationManager.render(this.uiSpritebatch);
        uiSpritebatch.end();
    }


    public void addNotification(String content, EMessageType type)
    {
        final MainGame _this = this;
        final String _content = content;
        final EMessageType _type = type;

        Gdx.app.postRunnable(new Runnable() {
            @Override
            public void run() {
                notificationManager.add(_this, _content, _type);
            }
        });


    }

    public void update()
    {
        this.notificationManager.update();
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

    public IHubService getHubService()
    {
        return hubService;
    }

    public String getUserPseudo(){
        return userPseudo;
    }

    public Sprite getPlayerSprite(){

        return new Sprite(this.playerTexture);
    }

    public BitmapFont getFont()
    {
        return new BitmapFont();
    }


}
