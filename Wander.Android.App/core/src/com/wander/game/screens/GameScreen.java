package com.wander.game.screens;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.maps.MapProperties;
import com.badlogic.gdx.maps.tiled.TiledMap;
import com.badlogic.gdx.maps.tiled.TiledMapRenderer;
import com.badlogic.gdx.maps.tiled.TmxMapLoader;
import com.badlogic.gdx.maps.tiled.renderers.OrthogonalTiledMapRenderer;
import com.badlogic.gdx.math.Vector3;
import com.wander.game.Camera;
import com.wander.game.Constants;
import com.wander.game.GameMap;
import com.wander.game.KeyBoardInputManager;
import com.wander.game.MainGame;
import com.wander.game.models.NotificationMessage;
import com.wander.game.models.PlayerModel;

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


    public GameScreen(MainGame game)
    {
        this.game = game;

        this.game.getHubService().getHub().on("notify", new SubscriptionHandler1<NotificationMessage>() {

            @Override
            public void run(NotificationMessage o) {
                System.out.println("NOTIFICATION FROM SERVER" + " : " + o.Content);
            }

        }, NotificationMessage.class);

        this.game.getHubService().getHub().on("playerConnected", new SubscriptionHandler1<PlayerModel>() {
            @Override
            public void run(PlayerModel o) {
                map.AddPlayer(o);
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
                map.updatePlayer(o);
            }
        }, PlayerModel.class);



    }

    @Override
    public void show() {
        inputManager = new KeyBoardInputManager(this);

        this.batch = new SpriteBatch();
        this.map = new GameMap("maps/map2.tmx", this);
        camera = new Camera(this);


        Gdx.input.setInputProcessor(this.inputManager);

        System.out.println(Constants.MAP_SIZE * Constants.TILE_SIZE * camera.getCamera().zoom);
        this.getCameraManager().getCamera().position.set(new Vector3(this.getCameraManager().getCamera().position.x, (Constants.MAP_SIZE * Constants.TILE_SIZE * camera.getCamera().zoom) * map.getScaleRatio() - this.camera.getCamera().viewportWidth / 2, 0));
        System.out.println(camera.getCamera().position);
        System.out.println(camera.getCamera().zoom);
        this.getCameraManager().getCamera().update();
    }

    @Override
    public void render(float delta) {
        this.update();
        batch.setProjectionMatrix(camera.getCamera().combined);
        Gdx.gl.glClearColor(0, 0, 1, 1);
        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
        map.draw(this.batch);

    }

    public void update()
    {
        map.update();
        inputManager.update();
        camera.update();
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
