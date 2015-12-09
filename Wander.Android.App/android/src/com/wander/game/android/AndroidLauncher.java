package com.wander.game.android;

import android.os.Bundle;

import com.badlogic.gdx.backends.android.AndroidApplication;
import com.badlogic.gdx.backends.android.AndroidApplicationConfiguration;
import com.wander.game.Wander;
import com.wander.game.services.AndroidHubService;
import com.wander.game.services.IHubService;


public class AndroidLauncher extends AndroidApplication {
	@Override
	protected void onCreate (Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		AndroidApplicationConfiguration config = new AndroidApplicationConfiguration();
		IHubService hubService = new AndroidHubService("http://wander.nightlydev.fr/", "GameHub");
		initialize(new Wander(hubService), config);
	}
}
