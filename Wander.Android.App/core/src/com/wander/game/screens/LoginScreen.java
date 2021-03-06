package com.wander.game.screens;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.GlyphLayout;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureAtlas;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Button;
import com.badlogic.gdx.scenes.scene2d.ui.Label;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.Table;
import com.badlogic.gdx.scenes.scene2d.ui.TextArea;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.badlogic.gdx.scenes.scene2d.utils.ClickListener;
import com.badlogic.gdx.utils.viewport.ScreenViewport;
import com.badlogic.gdx.utils.viewport.Viewport;
import com.wander.game.Constants;
import com.wander.game.MainGame;
import com.wander.game.util;


/**
 * Created by valentin on 07/12/2015.
 */
public class LoginScreen implements Screen {

    private MainGame game;
    private Stage stage;
    private Skin skin;
    private SpriteBatch batch;
    private TextArea loginTextArea;
    private TextArea passwordTextArea;
    private Button loginButton;
    private Sprite backgroundSprite;
    private Table table;

    public LoginScreen(MainGame game)
    {
        this.batch = new SpriteBatch();
        this.stage = new Stage();
        this.game = game;

        skin = new Skin(Gdx.files.internal("uiskin.json"));
        table = new Table(skin);
        table.setSize(Gdx.graphics.getWidth(), Gdx.graphics.getHeight());

        float width = Gdx.graphics.getWidth();
        float height = Gdx.graphics.getHeight();


        loginTextArea = new TextArea("login", skin);

        table.add(loginTextArea).size(width * 0.8f, height * 0.2f).padTop(10).padBottom(10);
        table.row();

        passwordTextArea = new TextArea("password", skin);
        passwordTextArea.setPasswordMode(true);
        passwordTextArea.setPasswordCharacter('*');

        table.add(passwordTextArea).size(width * 0.8f, height * 0.2f).padTop(10).padBottom(10);
        table.row();


        loginButton = new TextButton("Login", skin);

        loginButton.addListener(new ClickListener() {
            @Override
            public void clicked(InputEvent event, float x, float y) {
                login();
            }
        });

        table.add(loginButton).width(Constants.BTN_MENU_WIDTH).height(Constants.BTN_MENU_HEIGHT).padTop(Constants.BTN_MENU_PADDING).padBottom(Constants.BTN_MENU_PADDING);
        table.row();

        table.center();

        stage.addActor(table);



    }
    @Override
    public void show() {
        backgroundSprite = util.GetBackgroundSprite();
        backgroundSprite.setSize(Gdx.graphics.getWidth(), Gdx.graphics.getHeight());

        Gdx.input.setInputProcessor(this.stage);
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

    public void login()
    {
        System.out.println("Login : " + loginTextArea.getText() + " pass : " + passwordTextArea.getText());
        String pseudo = loginTextArea.getText();
        String pass = passwordTextArea.getText();
        boolean success = game.getHubService().connect(pseudo, pass);

        if(success) {
            game.SetConnected(pseudo);
        }
    }
}
