package com.wander.game.desktop;

import com.badlogic.gdx.backends.lwjgl.LwjglApplication;
import com.badlogic.gdx.backends.lwjgl.LwjglApplicationConfiguration;
import com.wander.game.Wander;
import com.wander.game.services.HubService;
import com.wander.game.services.IHubService;

public class DesktopLauncher {
	public static void main (String[] arg) {
		LwjglApplicationConfiguration config = new LwjglApplicationConfiguration();
		config.width = 1024;
		config.height = 768;
		IHubService hubService = new HubService("http://wander.nightlydev.fr/", "GameHub");
		new LwjglApplication(new Wander(hubService), config);
	}
}
