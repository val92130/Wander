package com.wander.game;

import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShaderProgram;
import com.badlogic.gdx.maps.tiled.TiledMap;
import com.badlogic.gdx.maps.tiled.TiledMapTileLayer;
import com.badlogic.gdx.maps.tiled.TmxMapLoader;
import com.badlogic.gdx.maps.tiled.renderers.OrthogonalTiledMapRenderer;
import com.badlogic.gdx.math.Rectangle;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;
import com.wander.game.screens.GameScreen;

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
    }

    public void update() {

    }


    public void draw(SpriteBatch batch) {
        mapRenderer.setView(game.getCameraManager().getCamera());

        mapRenderer.getBatch().begin();
        mapRenderer.renderTileLayer(this.getBackgroundLayer());
        mapRenderer.renderTileLayer(this.getObjectsLayer());

        mapRenderer.getBatch().end();

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
}
