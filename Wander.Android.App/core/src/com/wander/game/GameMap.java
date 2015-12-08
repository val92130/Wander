package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShaderProgram;
import com.badlogic.gdx.maps.tiled.TiledMap;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer;
import com.badlogic.gdx.maps.tiled.TmxMapLoader;
import com.badlogic.gdx.maps.tiled.renderers.OrthogonalTiledMapRenderer;
import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.wander.game.models.ClientPlayer;
import com.wander.game.models.Player;
import com.wander.game.models.PlayerModel;
import com.wander.game.models.ServerPlayer;
import com.wander.game.screens.GameScreen;

import java.util.ArrayList;
import java.util.HashMap;

/**
 * Created by val on 07/12/2015.
 */
public class GameMap {
    private GameScreen game;
    private String fileName;
    private TiledMap map;
    private OrthogonalTiledMapRenderer mapRenderer;
    private int ratio = 4;
    private TiledMapTileLayer backgroundLayer,collisionLayer,lightsLayer,objectsLayer,houseLayer;
    private ArrayList<ServerPlayer> players;
    private ClientPlayer currentPlayer;

    public GameMap(String fileName, GameScreen game) {

        this.game = game;
        this.fileName = fileName;
        map = new TmxMapLoader().load(fileName);
        mapRenderer = new OrthogonalTiledMapRenderer(getMap(), ratio);
        ShaderProgram.pedantic = false;

        this.backgroundLayer = (TiledMapTileLayer) map.getLayers().get("backgroundLayer");
        this.collisionLayer = (TiledMapTileLayer) map.getLayers().get("collisionLayer");
        this.lightsLayer = (TiledMapTileLayer) map.getLayers().get("lightsLayer");
        this.objectsLayer = (TiledMapTileLayer) map.getLayers().get("objectsLayer");
        this.houseLayer = (TiledMapTileLayer) map.getLayers().get("houseLayer");


        this.players = new ArrayList<ServerPlayer>();
        this.currentPlayer = new ClientPlayer(this, this.game.getMainGame().getUserPseudo(), new Vector2(0,0),game.getMainGame().getPlayerSprite());
        this.game.getMainGame().getHubService().getHub().invoke("GetAllPlayers");
    }

    public void update() {
        for (int i = 0; i < this.players.size(); i++) {
            this.players.get(i).update(Gdx.graphics.getDeltaTime());
        }
        this.currentPlayer.update(Gdx.graphics.getDeltaTime());
        this.getGameScreen().getCameraManager().follow(new Vector2(currentPlayer.getSprite().getX(), currentPlayer.getSprite().getY()));
    }


    public void draw(SpriteBatch batch) {
        mapRenderer.setView(game.getCameraManager().getCamera());

        mapRenderer.getBatch().begin();
        mapRenderer.renderTileLayer(this.getBackgroundLayer());
        mapRenderer.renderTileLayer(this.getObjectsLayer());
        for(int i = 0; i < this.players.size(); i++)
        {
            this.players.get(i).render((SpriteBatch) mapRenderer.getBatch());
        }
        this.currentPlayer.render((SpriteBatch) mapRenderer.getBatch());
        mapRenderer.getBatch().end();


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
            if(this.players.get(i).getPseudo().equals( p.Pseudo)){
                this.players.get(i).updateInfos(new Vector2(p.Position.X, p.Position.Y), p.Direction);
                return true;
            }
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
}
