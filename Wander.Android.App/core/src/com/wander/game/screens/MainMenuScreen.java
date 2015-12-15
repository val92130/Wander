package com.wander.game.screens;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Label;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.Table;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.badlogic.gdx.scenes.scene2d.utils.ClickListener;
import com.wander.game.AssetManager;
import com.wander.game.Constants;
import com.wander.game.MainGame;
import com.wander.game.util;

/**
 * Created by val on 07/12/2015.
 */
public class MainMenuScreen implements Screen {

    private MainGame game;
    private Stage stage;
    private Table table;
    private Skin skin;
    private Sprite backgroundSprite;

    public MainMenuScreen(final MainGame game)
    {
        this.game = game;
        this.stage = new Stage();
        skin = new Skin(Gdx.files.internal("uiskin.json"));
        table = new Table(skin);
        table.setSize(Gdx.graphics.getWidth(), Gdx.graphics.getHeight());

        Label pseudoLabel = new Label(game.getUserPseudo(), skin);
        table.add(pseudoLabel).top();
        table.row();

        TextButton startGame=new TextButton("Start Game", AssetManager.getTextButtonStyle());

        startGame.addListener(new ClickListener() {
            @Override
            public void clicked(InputEvent event, float x, float y) {
                startGame();
            }
        });


        table.add(startGame).width(Constants.BTN_MENU_WIDTH).height(Constants.BTN_MENU_HEIGHT).padTop(Constants.BTN_MENU_PADDING).padBottom(Constants.BTN_MENU_PADDING / 4);
        table.row();

        TextButton playersButton=new TextButton("Players",AssetManager.getTextButtonStyle());
        table.add(playersButton).width(Constants.BTN_MENU_WIDTH).height(Constants.BTN_MENU_HEIGHT).padTop(Constants.BTN_MENU_PADDING).padBottom(Constants.BTN_MENU_PADDING / 4);
        table.row();

        TextButton logoutButton=new TextButton("Logout",AssetManager.getTextButtonStyle());
        logoutButton.addListener(new ClickListener()
        {
            public void clicked(InputEvent event, float x, float y) {
                game.logout();
            }
        });
        table.add(logoutButton).width(Constants.BTN_MENU_WIDTH).height(Constants.BTN_MENU_HEIGHT).padTop(Constants.BTN_MENU_PADDING).padBottom(Constants.BTN_MENU_PADDING / 4);
        table.row();

        table.center();
        stage.addActor(table);
        Gdx.input.setInputProcessor(this.stage);

    }
    @Override
    public void show() {


        backgroundSprite = util.GetBackgroundSprite();
        backgroundSprite.setSize(Gdx.graphics.getWidth(), Gdx.graphics.getHeight());
    }

    @Override
    public void render(float delta) {
        Gdx.gl.glClearColor(0, 0, 0.25f, 1);
        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);

        stage.act(Gdx.graphics.getDeltaTime());
        this.game.batch.begin();
        backgroundSprite.draw(this.game.batch);
        this.game.batch.end();
        stage.draw();
    }

    public void startGame()
    {
        System.out.println("Starting game");
        this.game.startGameScreen();
    }

    @Override
    public void resize(int width, int height) {
        this.stage.getViewport().setScreenSize(width,height);
        this.table.setSize(width,height);
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
