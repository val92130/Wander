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
    private SpriteBatch batch;
    private Sprite backgroundSprite;

    public MainMenuScreen(MainGame game)
    {
        this.game = game;
        this.stage = new Stage();
        skin = new Skin(Gdx.files.internal("uiskin.json"));
        table = new Table(skin);
        table.setSize(Gdx.graphics.getWidth(), Gdx.graphics.getHeight());

        Label pseudoLabel = new Label(game.getUserPseudo(), skin);
        table.add(pseudoLabel).top();
        table.row();

        TextButton startGame=new TextButton("Start Game",skin);

        startGame.addListener(new ClickListener() {
            @Override
            public void clicked(InputEvent event, float x, float y) {
                startGame();
            }
        });


        table.add(startGame).width(Constants.BTN_MENU_WIDTH).height(Constants.BTN_MENU_HEIGHT).padTop(Constants.BTN_MENU_PADDING).padBottom(Constants.BTN_MENU_PADDING / 4);
        table.row();

        TextButton playersButton=new TextButton("Players",skin);
        table.add(playersButton).width(Constants.BTN_MENU_WIDTH).height(Constants.BTN_MENU_HEIGHT).padTop(Constants.BTN_MENU_PADDING).padBottom(Constants.BTN_MENU_PADDING);
        table.row();

        table.center();
        stage.addActor(table);
        Gdx.input.setInputProcessor(this.stage);
        batch = new SpriteBatch();


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
        batch.begin();
        backgroundSprite.draw(batch);
        batch.end();
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
