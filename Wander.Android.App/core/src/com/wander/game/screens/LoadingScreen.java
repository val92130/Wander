package com.wander.game.screens;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.SelectBox;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.Table;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.badlogic.gdx.scenes.scene2d.utils.ClickListener;
import com.wander.game.AssetManager;
import com.wander.game.Constants;
import com.wander.game.MainGame;
import com.wander.game.util;

import microsoft.aspnet.signalr.client.ConnectionState;

/**
 * Created by val on 12/12/2015.
 */
public class LoadingScreen implements Screen {

    private MainGame game;
    private Skin skin;
    private BitmapFont font;
    private boolean error = false;
    private Stage stage;
    private Table table;

    public LoadingScreen(MainGame game)
    {
        this.game = game;
    }

    @Override
    public void show() {
        float width = Gdx.graphics.getWidth();
        float height = Gdx.graphics.getHeight();

        this.skin = AssetManager.getSkin();
        this.font = this.skin.getFont("default-font");
        this.stage = new Stage();

        this.table = new Table(skin);
        this.table.setSize(Gdx.graphics.getWidth(), Gdx.graphics.getHeight());
        TextButton t = new TextButton("Retry", this.skin);
        t.addListener(new ClickListener() {
            @Override
            public void clicked(InputEvent event, float x, float y) {
                game.retryConnection();
            }
        });
        this.table.add(t).width(Constants.BTN_MENU_WIDTH / 4).height(Constants.BTN_MENU_HEIGHT);
        this.table.center();
        this.stage.addActor(this.table);
        Gdx.input.setInputProcessor(this.stage);

    }

    @Override
    public void render(float delta) {

        Gdx.gl.glClearColor(0, 0, 0, 1);
        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);

        String str = "Loading...";
        float fontWidth = util.GetStringWidth(this.font, str);
        float fontHeight = util.GetStringHeight(this.font, str);

        float x = (Gdx.graphics.getWidth() / 2) - (fontWidth/2);
        float y = (Gdx.graphics.getHeight() / 2) + (fontHeight/2) + Constants.BTN_MENU_PADDING;

        this.game.batch.begin();
        if(this.error || this.game.getHubService().getConnection().getState() != ConnectionState.Connected)
        {
            this.stage.act(delta);
            this.stage.draw();
            float fontWidthEr = util.GetStringWidth(this.font, "Connection error");
            float fontHeightEr = util.GetStringHeight(this.font, "Connection error");

            float xEr = (Gdx.graphics.getWidth() / 2) - (fontWidthEr/2);
            float yEr = (Gdx.graphics.getHeight() / 2) + (fontHeightEr/2) + Constants.BTN_MENU_PADDING;

            this.font.setColor(Color.RED);
            this.font.draw(this.game.batch, "Connection error", xEr, yEr);
            this.font.setColor(Color.WHITE);
        } else
        {
            this.font.draw(this.game.batch, str, x, y);
        }
        this.game.batch.end();
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

}
