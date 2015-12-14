package com.wander.game.weather;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShaderProgram;
import com.badlogic.gdx.math.Vector3;
import com.wander.game.GameMap;
import com.wander.game.screens.GameScreen;

/**
 * Created by val on 10/12/2015.
 */
public class AmbientManager {

    private ShaderProgram ambientShader;
    private float ambientIntensity = 1f;
    private Vector3 ambientColor = new Vector3(0.3f, 0.3f, 0.6f);
    private Vector3 oldAmbientColor = new Vector3(1, 1, 1);
    private boolean isNightTime = false;
    private boolean isRaining = false;
    private long startTime, currentTime;
    long elapsedSeconds = 0;
    public int timeSpeed = 1;
    private GameMap map;
    private SoundManager soundManager;

    public AmbientManager(GameMap map) {
        startTime = System.nanoTime();
        currentTime = System.nanoTime();

        this.map = map;
        ambientShader = new ShaderProgram(Gdx.files.internal("shaders/passthrough.vsh.glsl"), Gdx.files.internal("shaders/ambient.fsh.glsl"));
        if (!ambientShader.isCompiled()) {
            System.out.println(ambientShader.getLog());
        }
        this.map.getMapRenderer().getBatch().setShader(ambientShader);
        soundManager = new SoundManager(this.map.getGameScreen().getMainGame());

    }

    public void render(SpriteBatch batch) {
        /* NIGHT SIMULATION */
        ambientShader.begin();
        if (isNightTime) {
            oldAmbientColor = oldAmbientColor.lerp(ambientColor, 0.01f);
            ambientShader.setUniformf("ambientColor", oldAmbientColor.x, oldAmbientColor.y,
                    oldAmbientColor.z, ambientIntensity);
        } else {
            oldAmbientColor = oldAmbientColor.lerp(new Vector3(1, 1, 1), 0.01f);
            ambientShader.setUniformf("ambientColor", oldAmbientColor.x, oldAmbientColor.y,
                    oldAmbientColor.z, 1);

        }
        ambientShader.end();

        /* END NIGHT SIMULATION */

    }

    public void update() {
        /* TIME SIMULATION */
        if (((currentTime - startTime) / 1000) >= 1000000) {
            startTime = System.nanoTime();
            elapsedSeconds += timeSpeed;
        }

        currentTime = System.nanoTime();
        this.soundManager.update();
        /* END TIME SIMULATION */
    }

    public void dispose(){
        this.soundManager.dispose();
        this.ambientShader.dispose();
    }

    public void setNightTime(boolean value) {
        isNightTime = value;
    }

    public boolean getNightTime() {
        return isNightTime;
    }

    public long getCurrentTime() {
        return elapsedSeconds;
    }

    public void setRaining(boolean val)
    {
        this.isRaining = val;
    }

    public boolean isDay(){return !isNightTime;}
    public boolean isNight(){return isNightTime;}
    public boolean isRaining(){return isRaining;}
}