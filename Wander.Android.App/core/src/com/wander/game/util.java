package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.GlyphLayout;
import com.badlogic.gdx.graphics.g2d.Sprite;

/**
 * Created by val on 07/12/2015.
 */
public class util {

    static GlyphLayout layout = new GlyphLayout();
    static Sprite backgroundSprite;
    public static float GetStringWidth(BitmapFont font, String charSeq)
    {
        layout.setText(font, charSeq);
        return layout.width;
    }

    public static float GetStringHeight(BitmapFont font, String charSeq)
    {
        layout.setText(font, charSeq);
        return layout.height;
    }

    public static Sprite GetBackgroundSprite()
    {
        if(backgroundSprite == null)
        {
            backgroundSprite = new Sprite(new Texture(Gdx.files.internal("images/background.jpg")));
        }
        return backgroundSprite;
    }

    public static float Lerp(float goal, float current, float time) {
        float diff = goal - current;

        if (diff > time) {
            return current + time;
        }
        if (diff < -time) {
            return current - time;
        }
        return goal;
    }
}
