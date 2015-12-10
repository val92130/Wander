package com.wander.game.screens;

import com.badlogic.gdx.Application;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.InputMultiplexer;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.input.GestureDetector;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.Touchpad;
import com.badlogic.gdx.scenes.scene2d.utils.Drawable;
import com.badlogic.gdx.utils.viewport.ScreenViewport;
import com.wander.game.Camera;
import com.wander.game.Constants;
import com.wander.game.GameMap;
import com.wander.game.InputHandling.KeyBoardInputManager;
import com.wander.game.InputHandling.TouchInputManager;
import com.wander.game.MainGame;
import com.wander.game.models.ClientPlayer;
import com.wander.game.models.EMessageType;
import com.wander.game.models.NotificationMessage;
import com.wander.game.models.PlayerModel;

import javax.xml.ws.Dispatch;

import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;

/**
 * Created by val on 07/12/2015.
 */
public class GameScreen implements Screen {

    private MainGame game;
    private Camera camera;
    private GameMap map;
    private SpriteBatch batch;
    private KeyBoardInputManager inputManager;
    private Touchpad touchpad;
    private Touchpad.TouchpadStyle touchpadStyle;
    private Skin touchpadSkin;
    private Drawable touchBackground;
    private Drawable touchKnob;
    private Stage stage;


    public GameScreen(final MainGame game)
    {
        this.game = game;

        this.game.getHubService().getHub().on("playerConnected", new SubscriptionHandler1<PlayerModel>() {
            @Override
            public void run(PlayerModel o) {
                System.out.println("player connected " + o);
                final PlayerModel p = o;
                Gdx.app.postRunnable(new Runnable() {
                    @Override
                    public void run() {
                        map.addPlayer(p);
                    }
                });

            }
        }, PlayerModel.class);

        this.game.getHubService().getHub().on("playerDisconnected", new SubscriptionHandler1<PlayerModel>() {
            @Override
            public void run(PlayerModel o) {
                final PlayerModel p = o;
                Gdx.app.postRunnable(new Runnable() {
                    @Override
                    public void run() {
                        map.removePlayer(p);
                    }
                });
            }
        }, PlayerModel.class);

        this.game.getHubService().getHub().on("onConnected", new SubscriptionHandler1<Object>() {
            @Override
            public void run(Object o) {
                System.out.println("On connected" + o);
            }
        }, Object.class);

        this.game.getHubService().getHub().on("playerMoved", new SubscriptionHandler1<PlayerModel>() {
            @Override
            public void run(PlayerModel o) {
                final PlayerModel p = o;
                Gdx.app.postRunnable(new Runnable() {
                    @Override
                    public void run() {
                        map.updatePlayer(p);
                    }
                });
            }
        }, PlayerModel.class);

    }

    @Override
    public void show() {
        this.batch = new SpriteBatch();
        this.map = new GameMap("maps/map2.tmx", this);
        camera = new Camera(this);

        InputMultiplexer im = new InputMultiplexer();
        GestureDetector gd = new GestureDetector(new TouchInputManager(this));
        im.addProcessor(gd);
        im.addProcessor(new KeyBoardInputManager(this));
        inputManager = new KeyBoardInputManager(this);

        System.out.println(Constants.MAP_SIZE * Constants.TILE_SIZE * camera.getCamera().zoom);
        this.getCameraManager().getCamera().position.set(new Vector3(this.getCameraManager().getCamera().position.x, (Constants.MAP_SIZE * Constants.TILE_SIZE * camera.getCamera().zoom) * map.getScaleRatio() - this.camera.getCamera().viewportWidth / 2, 0));
        System.out.println(camera.getCamera().position);
        System.out.println(camera.getCamera().zoom);
        this.getCameraManager().getCamera().update();

        touchpadSkin = new Skin();
        touchpadSkin.add("touchBackground", new Texture("images/touchBackground.png"));
        touchpadSkin.add("touchKnob", new Texture("images/touchKnob.png"));
        touchpadStyle = new Touchpad.TouchpadStyle();
        touchBackground = touchpadSkin.getDrawable("touchBackground");
        touchKnob = touchpadSkin.getDrawable("touchKnob");
        touchpadStyle.background = touchBackground;
        touchpadStyle.knob = touchKnob;
        touchpad = new Touchpad(10, touchpadStyle);
        touchpad.setBounds(15, 15, Gdx.graphics.getHeight()/3, Gdx.graphics.getHeight()/3);

        stage = new Stage(new ScreenViewport(), batch);
        if(Gdx.app.getType() == Application.ApplicationType.Android) stage.addActor(touchpad);
        im.addProcessor(stage);
        Gdx.input.setInputProcessor(im);

    }

    @Override
    public void render(float delta) {
        this.update();
        batch.setProjectionMatrix(camera.getCamera().combined);
        Gdx.gl.glClearColor(0, 0, 1, 1);
        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
        map.draw(this.batch);
        stage.draw();
    }

    public void update()
    {
        map.update();
        inputManager.update();
        camera.update();
        stage.act();
        if(Gdx.app.getType() == Application.ApplicationType.Android){
            map.getCurrentPlayer().moveOffset(new Vector2(touchpad.getKnobPercentX() * ClientPlayer.speed * Gdx.graphics.getDeltaTime(), touchpad.getKnobPercentY() * ClientPlayer.speed * Gdx.graphics.getDeltaTime()));
        }
    }

    public Camera getCameraManager()
    {
        return camera;
    }

    public GameMap getMap()
    {
        return map;
    }

    public MainGame getMainGame(){ return game;}

    @Override
    public void resize(int width, int height) {

    }

    @Override
    public void pause() {

    }

    @Override
    public void resume() {

    }

    @Override
    public void hide() {

    }

    @Override
    public void dispose() {

    }
}
