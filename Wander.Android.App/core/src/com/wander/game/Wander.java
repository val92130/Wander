package com.wander.game;
import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;

public class Wander extends ApplicationAdapter {
	SpriteBatch batch;
	MainGame game;
	
	@Override
	public void create () {
		game = new MainGame();
		batch = new SpriteBatch();
	}

	@Override
	public void render () {
		game.update();
		Gdx.gl.glClearColor(0, 0, 0, 1);
		Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
		batch.begin();
		game.render();
		batch.end();
	}
}
