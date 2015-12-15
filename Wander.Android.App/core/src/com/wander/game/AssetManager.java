package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.TextureAtlas;
import com.badlogic.gdx.scenes.scene2d.ui.ScrollPane;
import com.badlogic.gdx.scenes.scene2d.ui.SelectBox;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.badlogic.gdx.scenes.scene2d.ui.TextField;

/**
 * Created by val on 13/12/2015.
 */
public class AssetManager {

    private static Skin skin;
    private static TextureAtlas uiAtlas = new TextureAtlas(Gdx.files.internal("ui/ui-orange.atlas"));
    private static Skin uiSkin = new Skin(uiAtlas);

    public static Skin getSkin()
    {
        if(skin == null)
        {
            skin = new Skin(Gdx.files.internal("uiskin.json"));
        }
        return skin;
    }

    public static TextButton.TextButtonStyle getTextButtonStyle()
    {
        return new TextButton.TextButtonStyle(uiSkin.getDrawable("button_02"),uiSkin.getDrawable("button_03"),uiSkin.getDrawable("button_02"),getSkin().getFont("default-font"));
    }

    public static TextField.TextFieldStyle getTextFieldStyle(){
        return new TextField.TextFieldStyle(getSkin().getFont("default-font"), Color.BLACK, uiSkin.getDrawable("textbox_cursor_02"), uiSkin.getDrawable("textbox_cursor_02"), uiSkin.getDrawable("textbox_01"));
    }


    public static BitmapFont getFontSmall()
    {
        BitmapFont smallFnt = new BitmapFont(Gdx.files.internal("wanderFnt.fnt"));
        smallFnt.getData().setScale(0.5f, 0.5f);
        return smallFnt;
    }
}
