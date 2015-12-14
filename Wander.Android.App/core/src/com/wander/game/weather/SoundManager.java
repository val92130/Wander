package com.wander.game.weather;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.audio.Music;
import com.wander.game.MainGame;
import com.wander.game.util;

/**
 * Created by val on 12/12/2015.
 */
public class SoundManager {
    private Music dayAmbientSound;
    private Music nightAmbientSound;
    private Music rainAmbientSound;
    private MainGame game;

    public SoundManager(MainGame game)
    {
        this.game = game;
        this.dayAmbientSound = Gdx.audio.newMusic(Gdx.files.internal("sounds/ambient-day.mp3"));
        this.nightAmbientSound = Gdx.audio.newMusic(Gdx.files.internal("sounds/ambient-night.mp3"));
        this.rainAmbientSound = Gdx.audio.newMusic(Gdx.files.internal("sounds/ambient-rain.mp3"));

        this.dayAmbientSound.setVolume(0);
        this.nightAmbientSound.setVolume(0);
        this.rainAmbientSound.setVolume(0);

        this.dayAmbientSound.setLooping(true);
        this.nightAmbientSound.setLooping(true);
        this.rainAmbientSound.setLooping(true);
    }

    public void update(){

        if (this.game.getScreenManager().getGameScreen().getMap().getAmbientManager().isDay()) {
            if (!this.dayAmbientSound.isPlaying()) this.dayAmbientSound.play();
            this.nightAmbientSound.setVolume(util.Lerp(0, this.nightAmbientSound.getVolume(), 0.01f));
            this.dayAmbientSound.setVolume(util.Lerp(1, this.dayAmbientSound.getVolume(), 0.01f));
            if (this.nightAmbientSound.getVolume() <= 0 && this.nightAmbientSound.isPlaying()) this.nightAmbientSound.stop();

        } else {
            if (!this.nightAmbientSound.isPlaying()) this.nightAmbientSound.play();
            this.nightAmbientSound.setVolume(util.Lerp(1, this.nightAmbientSound.getVolume(), 0.01f));
            this.dayAmbientSound.setVolume(util.Lerp(0, this.dayAmbientSound.getVolume(), 0.01f));
            if (this.dayAmbientSound.getVolume() <= 0 && this.dayAmbientSound.isPlaying()) this.dayAmbientSound.stop();
        }

        if (this.game.getScreenManager().getGameScreen().getMap().getAmbientManager().isRaining()) {
            if (!this.rainAmbientSound.isPlaying()) this.rainAmbientSound.play();
            this.rainAmbientSound.setVolume(util.Lerp(1f, this.rainAmbientSound.getVolume(), 0.01f));
        } else {
            this.rainAmbientSound.setVolume(util.Lerp(0, this.rainAmbientSound.getVolume(), 0.01f));
            if (this.rainAmbientSound.getVolume() <= 0) {
                this.rainAmbientSound.stop();
            }
        }
    }

    public void dispose(){
        this.dayAmbientSound.dispose();
        this.nightAmbientSound.dispose();
        this.rainAmbientSound.dispose();
    }
}
