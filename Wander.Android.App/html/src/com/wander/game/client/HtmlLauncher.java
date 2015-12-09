package com.wander.game.client;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.backends.gwt.GwtApplication;
import com.badlogic.gdx.backends.gwt.GwtApplicationConfiguration;
import com.wander.game.Wander;
import com.wander.game.services.HubService;
import com.wander.game.services.IHubService;

public class HtmlLauncher extends GwtApplication {

        @Override
        public GwtApplicationConfiguration getConfig () {
                return new GwtApplicationConfiguration(480, 320);
        }

        @Override
        public ApplicationListener getApplicationListener () {
                IHubService hubService = new HubService("http://wander.nightlydev.fr/", "GameHub");
                return new Wander(hubService);
        }
}