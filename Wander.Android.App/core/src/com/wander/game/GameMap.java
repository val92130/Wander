package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShaderProgram;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.maps.tiled.TiledMap;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer;
import com.badlogic.gdx.maps.tiled.TmxMapLoader;
import com.badlogic.gdx.maps.tiled.renderers.OrthogonalTiledMapRenderer;
import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;
import com.badlogic.gdx.physics.box2d.Box2DDebugRenderer;
import com.badlogic.gdx.physics.box2d.World;
import com.badlogic.gdx.scenes.scene2d.ui.Cell;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.wander.game.models.WanderVector;
import com.wander.game.player.ClientPlayer;
import com.wander.game.models.MessageModel;
import com.wander.game.models.PlayerModel;
import com.wander.game.player.ServerPlayer;
import com.wander.game.screens.GameScreen;
import com.wander.game.weather.AmbientManager;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.ExecutionException;

import microsoft.aspnet.signalr.client.Action;

/**
 * Created by val on 07/12/2015.
 */
public class GameMap {
    private GameScreen game;
    private TiledMap map;
    private OrthogonalTiledMapRenderer mapRenderer;
    private int ratio = 4;
    private TiledMapTileLayer backgroundLayer,collisionLayer,lightsLayer,objectsLayer,houseLayer;
    private ArrayList<ServerPlayer> players;
    private ClientPlayer currentPlayer;
    private World world;
    private Box2DDebugRenderer debugRenderer;
    private AmbientManager ambientManager;
    private Timer updateTimer;
    private ArrayList<Vector2> propertyPositions;
    private ArrayList<Vector2> mairiePositions;
    private Texture moneyDecal;
    private Texture mairieDecal;

    public GameMap(String fileName, GameScreen game) {

        this.game = game;
        map = new TmxMapLoader().load(fileName);
        mapRenderer = new OrthogonalTiledMapRenderer(getMap(), ratio);
        ShaderProgram.pedantic = false;

        this.backgroundLayer = (TiledMapTileLayer) map.getLayers().get("backgroundLayer");
        this.collisionLayer = (TiledMapTileLayer) map.getLayers().get("collisionLayer");
        this.lightsLayer = (TiledMapTileLayer) map.getLayers().get("lightsLayer");
        this.objectsLayer = (TiledMapTileLayer) map.getLayers().get("objectsLayer");
        this.houseLayer = (TiledMapTileLayer) map.getLayers().get("houseLayer");

        world = new World(new Vector2(0, 0), true);
        debugRenderer = new Box2DDebugRenderer();

        this.players = new ArrayList<ServerPlayer>();

        final WanderVector _pos = new WanderVector();
        try {
            this.game.getMainGame().getHubService().getHub().invoke(WanderVector.class, "GetCurrentPosition").done(new Action<WanderVector>() {
                @Override
                public void run(WanderVector vec) throws Exception {
                    final WanderVector _vec = vec;
                    Gdx.app.postRunnable(new Runnable() {
                        @Override
                        public void run() {
                            _pos.X = _vec.X;
                            _pos.Y = Constants.MAP_SIZE * Constants.TILE_SIZE  - _vec.Y;
                            currentPlayer.setPosition(new Vector2(_pos.X, _pos.Y));
                        }
                    });
                }
            }).get();
        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch (ExecutionException e) {
            e.printStackTrace();
        }


        this.currentPlayer = new ClientPlayer(this, this.game.getMainGame().getUserPseudo(), new Vector2(0,0),game.getMainGame().getPlayerSprite());


        this.ambientManager = new AmbientManager(this);
        this.game.getMainGame().getHubService().getHub().invoke("Update");

        this.updateTimer = new Timer();
        this.updateTimer.scheduleAtFixedRate(new TimerTask() {
            @Override
            public void run() {
               updateServer();
            }
        }, 10000, 10000);

        this.propertyPositions = new ArrayList<Vector2>();
        this.mairiePositions = new ArrayList<Vector2>();

        for(int i = 0; i < this.getHouseLayer().getWidth(); i++)
        {
            for(int j = 0; j < this.getHouseLayer().getHeight(); j++)
            {
                try{
                    TiledMapTileLayer.Cell c = this.getHouseLayer().getCell(i, j);
                    if(c != null)
                    {
                        System.out.println("found house tile at : " + i + " : " + j);
                        if(c.getTile().getProperties().containsKey("propertyId"))
                        {
                            propertyPositions.add(new Vector2(i * Constants.TILE_SIZE * getScaleRatio(), j * Constants.TILE_SIZE * getScaleRatio()));
                        } else if(c.getTile().getProperties().containsKey("Mairie"))
                        {
                            this.mairiePositions.add(new Vector2(i * Constants.TILE_SIZE * getScaleRatio(), j * Constants.TILE_SIZE * getScaleRatio()));
                        }

                    }
                } catch(Exception e)
                {

                }
            }
        }

        this.moneyDecal = new Texture(Gdx.files.internal("images/money-decal.png"));
        this.mairieDecal = new Texture(Gdx.files.internal("images/job-decal.png"));
        this.game.getMainGame().getHubService().getHub().invoke("GetAllPlayers");

    }

    private void updateServer(){
        if(this.game.getMainGame().isConnected())
        {
            this.game.getMainGame().getHubService().getHub().invoke("Update");
        }

    }

    public void update() {
        for (int i = 0; i < this.players.size(); i++) {
            this.players.get(i).update(Gdx.graphics.getDeltaTime());
        }
        this.currentPlayer.update(Gdx.graphics.getDeltaTime());
        this.getGameScreen().getCameraManager().follow(new Vector2(currentPlayer.getSprite().getX() + currentPlayer.getSprite().getWidth() / 2, currentPlayer.getSprite().getY() + currentPlayer.getSprite().getHeight() / 2));
        this.ambientManager.update();
        world.step(Gdx.graphics.getDeltaTime(), 6, 2);
    }


    public void draw(SpriteBatch batch) {
        mapRenderer.setView(game.getCameraManager().getCamera());

        mapRenderer.getBatch().begin();
        mapRenderer.renderTileLayer(this.getBackgroundLayer());
        mapRenderer.renderTileLayer(this.getObjectsLayer());
        for(int i = 0; i < this.players.size(); i++) {
            this.players.get(i).render((SpriteBatch) mapRenderer.getBatch());
        }

        for (Vector2 v : this.propertyPositions) {
            mapRenderer.getBatch().draw(this.moneyDecal,v.x, v.y, Constants.TILE_SIZE * this.getScaleRatio(), Constants.TILE_SIZE * this.getScaleRatio() );
        }
        for (Vector2 v : this.mairiePositions) {
            mapRenderer.getBatch().draw(this.mairieDecal,v.x, v.y, Constants.TILE_SIZE * this.getScaleRatio(), Constants.TILE_SIZE * this.getScaleRatio() );
        }
        this.currentPlayer.render((SpriteBatch) mapRenderer.getBatch());
        mapRenderer.getBatch().end();


        batch.begin();
        debugRenderer.render(world, game.getCameraManager().getCamera().combined);
        batch.end();

        this.ambientManager.render((SpriteBatch) mapRenderer.getBatch());
    }

    public void dispose(){
        this.ambientManager.dispose();
    }

    public boolean addPlayer(PlayerModel p)
    {
        if(p == null) try {
            throw new Exception("Player is null !");
        } catch (Exception e) {
            e.printStackTrace();
        }
        for(int i = 0; i < this.players.size(); i++)
        {
            if(this.players.get(i).getPseudo().equals(p.Pseudo)){
                return false;
            }
        }

        ServerPlayer pl = new ServerPlayer(this, p.Pseudo, new Vector2(p.Position.X, p.Position.Y), game.getMainGame().getPlayerSprite());
        this.players.add(pl);
        return true;
    }


    public boolean removePlayer(PlayerModel p)
    {
        if(p == null) try {
            throw new Exception("Player is null !");
        } catch (Exception e) {
            e.printStackTrace();
        }
        for(int i = 0; i < this.players.size(); i++)
        {

            if(this.players.get(i).getPseudo().equals(p.Pseudo)){
                this.players.remove(i);
                return true;
            }
        }
        return false;
    }

    public boolean updatePlayer(PlayerModel p)
    {
        if(p == null) try {
            throw new Exception("Player is null !");
        } catch (Exception e) {
            e.printStackTrace();
        }
        for(int i = 0; i < this.players.size(); i++)
        {
            if(this.players.get(i).getPseudo().equals(p.Pseudo)) {
                this.players.get(i).updateInfos(new Vector2(p.Position.X, p.Position.Y), p.Direction);
                return true;
            }
        }
        return false;
    }

    public void messageReceived(MessageModel message)
    {
        if(message == null) throw new NullPointerException("Message is null");
        ServerPlayer candidate = this.getPlayer(message.UserName);
        if(candidate != null){
            candidate.setTextMessage(message.Content);
        } else if(message.UserName.equals(currentPlayer.getPseudo()))
        {
            currentPlayer.setTextMessage(message.Content);
        }

    }

    public void actionPressed(){
        int tileX = (int)(currentPlayer.getPosition().x / Constants.TILE_SIZE) + 1;
        int tileY = (int)(currentPlayer.getPosition().y / Constants.TILE_SIZE) - 1;
        int houseId = -1;
        try{
            String o = this.getHouseLayer().getCell(tileX, tileY).getTile().getProperties().get("propertyId").toString();
            houseId = Integer.parseInt(o);
        } catch(Exception e)
        {
            System.out.println("null");
        }
        if(houseId != -1)
        {
            System.out.println("found house : " + houseId);
        }
    }

    public ServerPlayer getPlayer(String pseudo)
    {
        for(int i = 0; i < this.players.size(); i++)
        {

            if(this.players.get(i).getPseudo().equals(pseudo)){
                return this.players.get(i);
            }
        }
        return null;
    }

    public ArrayList<ServerPlayer> getAllPlayers()
    {
        return this.players;
    }



    public boolean isCollision(int tileX, int tileY)
    {
        try{
            if(this.collisionLayer.getCell(tileX,tileY).getTile() != null){
                return true;
            }

        }
        catch(NullPointerException e)
        {
            return false;
        }
        return false;
    }

    static public boolean intersect(Rectangle rectangle1, Rectangle rectangle2, Rectangle intersection) {
        if (rectangle1.overlaps(rectangle2)) {
            intersection.x = Math.max(rectangle1.x, rectangle2.x);
            intersection.width = Math.min(rectangle1.x + rectangle1.width, rectangle2.x + rectangle2.width) - intersection.x;
            intersection.y = Math.max(rectangle1.y, rectangle2.y);
            intersection.height = Math.min(rectangle1.y + rectangle1.height, rectangle2.y + rectangle2.height) - intersection.y;
            return true;
        }
        return false;
    }


    public HashMap<TiledMapTileLayer.Cell, Vector2> getCollisionCells() {
        HashMap<TiledMapTileLayer.Cell, Vector2> cells = new HashMap<TiledMapTileLayer.Cell, Vector2>();
        for (int i = 0; i < getCollisionLayer().getWidth(); i++) {
            for (int j = 0; j < getCollisionLayer().getHeight(); j++) {
                TiledMapTileLayer.Cell c = this.getCollisionLayer().getCell(i, j);
                if (c != null) {
                    cells.put(c, new Vector2(i, j));
                }
            }
        }
        return cells;
    }

    public OrthogonalTiledMapRenderer getMapRenderer() {
        return this.mapRenderer;
    }

    public ClientPlayer getCurrentPlayer()
    {
        return this.currentPlayer;
    }
    public int getScaleRatio() {
        return ratio;
    }

    public TiledMapTileLayer.Cell getCellAt(int x, int y, TiledMapTileLayer layer) {
        return layer.getCell(x, y);
    }

    public TiledMapTileLayer getCollisionLayer() {
        return collisionLayer;
    }

    public TiledMapTileLayer getBackgroundLayer() {
        return backgroundLayer;
    }

    public TiledMapTileLayer getObjectsLayer() {
        return objectsLayer;
    }

    public TiledMapTileLayer getHouseLayer() {
        return houseLayer;
    }

    public TiledMapTileLayer getLightsLayer() {
        return lightsLayer;
    }

    public World getWorld(){return world;}

    public TiledMapTileLayer.Cell getClosestCell(int x, int y, TiledMapTileLayer layer) {
        Vector3 worldCoords = game.getCameraManager().getCamera().unproject(new Vector3(x, y, 0));
        Vector2 selectedTile = new Vector2(worldCoords.x / Constants.TILE_SIZE, worldCoords.y / Constants.TILE_SIZE);
        TiledMapTileLayer.Cell cell = layer.getCell((int) selectedTile.x / ratio, (int) selectedTile.y / ratio);
        return cell;
    }

    public TiledMap getMap() {
        return map;
    }

    public GameScreen getGameScreen(){
        return this.game;
    }
    public AmbientManager getAmbientManager(){return this.ambientManager;}
}
