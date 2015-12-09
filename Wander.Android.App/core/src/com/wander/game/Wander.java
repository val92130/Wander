package com.wander.game;
import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.wander.game.services.IHubService;

public class Wander extends ApplicationAdapter {
	MainGame game;

	public Wander()
	{

	}
	@Override
	public void create () {
		game = new MainGame();
		game.create();
	}

	@Override
	public void render()
	{
		game.render();
	}

	@Override
	public void dispose()
	{
		game.getHubService().getConnection().stop();
	}

}
