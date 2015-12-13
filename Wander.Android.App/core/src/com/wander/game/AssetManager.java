package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;

/**
 * Created by val on 13/12/2015.
 */
public class AssetManager {

    private static Skin skin;

    public static Skin getSkin()
    {
        if(skin == null)
        {
            skin = new Skin(Gdx.files.internal("uiskin.json"));
        }
        return skin;
    }
}
