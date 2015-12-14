package com.wander.game.screens;

import com.wander.game.MainGame;

/**
 * Created by val on 14/12/2015.
 */
public class ScreenManager {

    private MainGame game;
    private LoginScreen loginScreen;
    private MainMenuScreen mainMenuScreen;
    private LoadingScreen loadingScreen;
    private GameScreen gameScreen;

    public ScreenManager(MainGame game)
    {
        this.game = game;
    }


    public void switchToLoginScreen()
    {
        this.reset();
        if(this.loginScreen != null)this.loginScreen.dispose();
        this.loginScreen = new LoginScreen(this.game);
        this.game.setScreen(this.loginScreen);
    }

    public void switchToMenuScreen()
    {
        if(this.mainMenuScreen != null)this.mainMenuScreen.dispose();
        this.mainMenuScreen = new MainMenuScreen(this.game);
        this.game.setScreen(this.mainMenuScreen);
    }

    public void switchToLoadingScreen()
    {
        if(this.loadingScreen != null)this.loadingScreen.dispose();
        this.loadingScreen = new LoadingScreen(this.game);
        this.game.setScreen(this.loadingScreen);
    }

    public void switchToGameScreen()
    {
        if(this.gameScreen == null)this.gameScreen = new GameScreen(this.game);
        this.game.setScreen(this.gameScreen);
    }

    public GameScreen getGameScreen()
    {
        return this.gameScreen;
    }


    public void reset()
    {
        if(this.loadingScreen != null){
            this.loadingScreen.dispose();
            this.loadingScreen = null;
        }

        if(this.loginScreen != null) {
            this.loginScreen.dispose();
            this.loginScreen = null;
        }

        if(this.mainMenuScreen != null){
            this.mainMenuScreen.dispose();
            this.mainMenuScreen = null;
        }

        if(this.gameScreen != null){
            this.gameScreen.dispose();
            this.gameScreen = null;
        }
    }
}
