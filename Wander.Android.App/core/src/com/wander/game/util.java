package com.wander.game;

import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.GlyphLayout;

/**
 * Created by val on 07/12/2015.
 */
public class util {

    static GlyphLayout layout = new GlyphLayout();
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
}
