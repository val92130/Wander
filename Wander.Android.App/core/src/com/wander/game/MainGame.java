package com.wander.game;

import com.badlogic.gdx.Application;
import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.utils.Json;
import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.reflect.TypeToken;
import com.wander.game.dialogs.ChangeJobDialog;
import com.wander.game.models.EMessageType;
import com.wander.game.models.JobModel;
import com.wander.game.models.MessageModel;
import com.wander.game.models.NotificationMessage;
import com.wander.game.models.ServerPropertyModel;
import com.wander.game.screens.GameScreen;
import com.wander.game.screens.LoadingScreen;
import com.wander.game.screens.LoginScreen;
import com.wander.game.screens.MainMenuScreen;
import com.wander.game.screens.ScreenManager;
import com.wander.game.services.HubService;
import com.wander.game.services.IHubService;

import java.lang.reflect.Array;
import java.util.ArrayList;

import microsoft.aspnet.signalr.client.Action;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;

/**
 * Created by valentin on 07/12/2015.
 */
public class MainGame extends Game {

    private IHubService hubService;
    private MainMenuScreen mainMenuScreen;
    private boolean connected;
    private String userPseudo;
    private Texture playerTexture = new Texture(Gdx.files.internal("images/player.png"));
    private NotificationManager notificationManager;
    public SpriteBatch batch;
    private ScreenManager screenManager;
    private Gson gson;

    public MainGame()
    {
        this.notificationManager = new NotificationManager(this);
        this.screenManager = new ScreenManager(this);
        this.gson = new Gson();

    }

    @Override
    public void create() {

        connectHub();

        this.batch = new SpriteBatch();


        this.getHubService().getHub().on("notify", new SubscriptionHandler1<NotificationMessage>() {

            @Override
            public void run(NotificationMessage o) {
                addNotification(o.Content, EMessageType.valueOf(o.MessageType));
                System.out.println("NOTIFICATION FROM SERVER" + " : " + o.Content);
            }

        }, NotificationMessage.class);


        this.getHubService().getHub().on("forceDisconnect", new SubscriptionHandler() {
            @Override
            public void run() {
                Gdx.app.postRunnable(new Runnable() {
                    @Override
                    public void run() {
                        onLogOut();
                    }
                });

            }
        });

        this.getHubService().getHub().on("MessageReceived", new SubscriptionHandler1<MessageModel>() {
            @Override
            public void run(MessageModel messageModel) {
                final MessageModel _message = messageModel;
                Gdx.app.postRunnable(new Runnable() {
                    @Override
                    public void run() {
                        messageReceived(_message);
                        System.out.println(_message);
                    }
                });
            }
        }, MessageModel.class);


    }

    public void connectHub(){
        this.hubService = new HubService(this,"http://wander.nightlydev.fr/", "GameHub");
        this.screenManager.switchToLoadingScreen();

        hubService.start();
    }


        @Override
    public void render() {
        this.update();
        super.render();

        batch.begin();
            this.notificationManager.render(this.batch);
        batch.end();
    }


    public void addNotification(String content, EMessageType type)
    {
        final MainGame _this = this;
        final String _content = content;
        final EMessageType _type = type;

        Gdx.app.postRunnable(new Runnable() {
            @Override
            public void run() {
                notificationManager.add(_content, _type);
            }
        });

    }

    public void messageReceived(MessageModel message)
    {
        if(message != null) this.screenManager.getGameScreen().getMap().messageReceived(message);
    }

    public void update()
    {
        this.notificationManager.update();
    }

    public void SetConnected(String pseudo)
    {
        this.connected = true;
        this.userPseudo = pseudo;
        this.screenManager.switchToMenuScreen();
    }

    public void openChangeJobDialog(final Stage stage)
    {
        if(!this.isConnected() || this.getScreenManager().getGameScreen() == null)return;
        final MainGame _this = this;
        this.getHubService().getHub().invoke(JsonElement.class, "GetAllJobs").done(new Action<JsonElement>() {

            @Override
            public void run(final JsonElement data) throws Exception {
                Gdx.app.postRunnable(new Runnable() {
                    @Override
                    public void run() {
                        System.out.println(data);
                        ArrayList<JobModel> jobs = gson.fromJson(data, new TypeToken<ArrayList<JobModel>>() {
                        }.getType());
                        _this.getScreenManager().getGameScreen().getModalManager().openChangeJobDialog(jobs);
                    }
                });
            }
        });
    }

    public void sendPublicMessage(String content)
    {
        if(content == null)return;
        if(content.length() == 0) return;
        this.getHubService().getHub().invoke("SendPublicMessage", content);
    }

    public void sendPrivateMessage(String content, String to)
    {
        if(to == null && content == null)return;
        if(to.length() == 0 && content.length() == 0) return;
        this.getHubService().getHub().invoke("SendPrivateMessage", content, to).done(new Action<Void>() {
            @Override
            public void run(Void aVoid) throws Exception {
                addNotification("Message sent ! ", EMessageType.success);
            }
        });
    }

    public void buyProperty(ServerPropertyModel model)
    {
        if(!isConnected())return;
        this.getHubService().getHub().invoke("BuyProperty", model.PropertyId);
    }

    public void changeJob(JobModel job)
    {
        if(!isConnected())return;
        this.getHubService().getHub().invoke("ApplyJob", job.JobId);
    }

    public void startGameScreen()
    {

        if(this.connected && this.userPseudo != null)
        {
            screenManager.switchToGameScreen();
        }
    }

    public void goToMainMenu(){
        if(this.isConnected())this.getScreenManager().switchToMenuScreen();
    }

    public void logout(){
        if(this.isConnected()) this.getHubService().getHub().invoke("Disconnect");
        this.connectHub();
        this.screenManager.switchToLoginScreen();
    }


    public void onConnectionEstablished()
    {
        screenManager.switchToLoginScreen();
    }

    public void retryConnection()
    {
        hubService.start();
    }

    public void onConnectionError(){
        final MainGame _this = this;
        Gdx.app.postRunnable(new Runnable() {

            @Override
            public void run() {
                screenManager.reset();
                screenManager.switchToLoadingScreen();
                _this.connected = false;
                /*
                if(_this.connected)
                {
                    if(_this.gameScreen != null){
                        _this.gameScreen.dispose();
                        _this.gameScreen = null;
                    }

                    _this.notificationManager.add("Connection error, please reconnect", EMessageType.error);
                    _this.setScreen(_this.loadingScreen);
                    _this.connected = false;
                } else
                {
                    _this.loadingScreen.onConnectionError();
                }
                */
            }
        });

    }
    public boolean isConnected(){return this.connected;};

    public void onLogOut(){
        this.logout();
    }

    public Gson getGson(){return this.gson;}
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

    public ScreenManager getScreenManager(){return this.screenManager;}

    public BitmapFont getFont()
    {
        return new BitmapFont();
    }


}
