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

    private ShaderProgram ambientShader, earthQuakeShader;
    private float ambientIntensity = .7f;
    private Vector3 ambientColor = new Vector3(0.3f, 0.3f, 0.7f);
    private Vector3 oldAmbientColor = new Vector3(1, 1, 1);
    private boolean isNightTime = false;
    private long startTime, currentTime;
    long elapsedSeconds = 0;
    public int timeSpeed = 1;
    private GameMap map;

    public AmbientManager(GameMap map) {
        startTime = System.nanoTime();
        currentTime = System.nanoTime();

        this.map = map;
        ambientShader = new ShaderProgram(Gdx.files.internal("shaders/passthrough.vsh.glsl"), Gdx.files.internal("shaders/ambient.fsh.glsl"));
        earthQuakeShader = new ShaderProgram(Gdx.files.internal("shaders/red.vsh.glsl"), Gdx.files.internal("shaders/red.fsh.glsl"));
        if (!ambientShader.isCompiled()) {
            System.out.println(ambientShader.getLog());
        }
        this.map.getMapRenderer().getBatch().setShader(ambientShader);

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

        /* END TIME SIMULATION */
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
}