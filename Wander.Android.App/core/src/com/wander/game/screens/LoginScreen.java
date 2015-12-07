package com.wander.game.screens;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.GlyphLayout;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Button;
import com.badlogic.gdx.scenes.scene2d.ui.Label;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.TextArea;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.badlogic.gdx.scenes.scene2d.utils.ClickListener;
import com.badlogic.gdx.utils.viewport.ScreenViewport;
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
    private Label errorLabel;
    private Button loginButton;
    private Sprite backgroundSprite;

    public LoginScreen(MainGame game)
    {
        this.batch = new SpriteBatch();
        this.stage = new Stage(new ScreenViewport());
        Gdx.input.setInputProcessor(this.stage);
        this.game = game;

        skin = new Skin(Gdx.files.internal("uiskin.json"));
        loginButton = new TextButton("Login", skin, "default");

        loginButton.setWidth(200f);
        loginButton.setHeight(20f);
        loginButton.setPosition(Gdx.graphics.getWidth() / 2 - 100f, Gdx.graphics.getHeight() / 2 - 10f);

        loginButton.addListener(new ClickListener() {
            @Override
            public void clicked(InputEvent event, float x, float y) {
                login();
            }
        });

        passwordTextArea = new TextArea("password", skin);
        passwordTextArea.setPasswordMode(true);
        passwordTextArea.setPasswordCharacter('*');
        passwordTextArea.setPosition(Gdx.graphics.getWidth() / 2 - passwordTextArea.getWidth() / 2, loginButton.getY() + loginButton.getHeight() + 20);

        loginTextArea = new TextArea("login", skin);
        loginTextArea.setPosition(Gdx.graphics.getWidth() / 2 - loginTextArea.getWidth()/2, passwordTextArea.getY() + loginTextArea.getHeight() + 20);

        errorLabel = new Label("", skin);
        errorLabel.setPosition(Gdx.graphics.getWidth() / 2 - errorLabel.getWidth()/2, loginButton.getY() - 10 - errorLabel.getHeight());
        errorLabel.setColor(Color.RED);

        stage.addActor(loginButton);
        stage.addActor(passwordTextArea);
        stage.addActor(loginTextArea);
        stage.addActor(errorLabel);

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

        if(!success) {

            String msg = "Wrong username/password";
            errorLabel.setText(msg);
            errorLabel.setColor(Color.RED);
            errorLabel.setPosition(Gdx.graphics.getWidth() / 2 - errorLabel.getWidth() / 2 - util.GetStringWidth(skin.getFont("default-font"), msg) / 2, loginButton.getY() - 10 - errorLabel.getHeight());
        } else{
            game.SetConnected(pseudo);
        }
    }
}
