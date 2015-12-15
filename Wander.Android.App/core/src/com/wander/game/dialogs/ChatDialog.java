package com.wander.game.dialogs;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Dialog;
import com.badlogic.gdx.scenes.scene2d.ui.Label;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.TextArea;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.badlogic.gdx.utils.Align;
import com.wander.game.Constants;
import com.wander.game.MainGame;
import com.wander.game.util;


/**
 * Created by val on 12/12/2015.
 */
public class ChatDialog extends Dialog {

    private MainGame game;
    private Stage stage;
    private String userDest;
    private TextArea area;

    public ChatDialog(MainGame game, Stage stage, Skin skin,String labelContent) {
        super("", skin);
        this.stage = stage;
        this.game = game;

        Label label = new Label(labelContent, skin);

        label.setWrap(true);
        label.setAlignment(Align.center);
        this.getContentTable().row();
        this.padTop(50).padBottom(50);
        this.getContentTable().add(label).width(850).padBottom(50).row();


        this.getButtonTable().padTop(50);
        this.getContentTable().row();

        TextButton dbutton = new TextButton("Send", skin);
        this.button(dbutton, true);

        this.area = new TextArea("", skin);
        this.area.setMaxLength(Constants.MAX_CHAT_TEXT_LENGTH);

        System.out.println(this.getWidth());
        this.getContentTable().add(area).center().fillX().height(util.GetStringHeight(skin.getFont("default-font"), "AAA") + 30);
        this.getContentTable().row();
        this.getContentTable().center();

        this.key(Input.Keys.ENTER, true).key(Input.Keys.ESCAPE, false);
        this.invalidateHierarchy();
        this.invalidate();
        this.layout();
        this.show(stage);

        float width = Gdx.graphics.getWidth();
        float height = Gdx.graphics.getHeight();

        this.setWidth(width * 0.7f);
        this.setHeight(height * 0.5f);
        this.setPosition(width / 2 - this.getWidth() / 2, height / 2 - this.getHeight() / 2);

        this.stage.setKeyboardFocus(this.area);
        this.area.setText("");

    }

    public ChatDialog(MainGame game, Stage stage, Skin skin,String labelContent, String userDest) {
        this(game,stage,skin,labelContent);
        this.userDest = userDest;
    }

    @Override
    protected void result (Object object) {
        try{
            boolean val = ((Boolean) object).booleanValue();
            if(val)
            {
                if(this.userDest != null)
                {
                    this.game.sendPrivateMessage(this.area.getText(), this.userDest);
                    System.out.println("Sending private message to " + this.userDest + "  : " + this.area.getText());
                } else
                {
                    this.game.sendPublicMessage(this.area.getText());
                    System.out.println("Sending message : " + this.area.getText());
                }

            }


        } catch(Exception e )
        {

        }
        this.setVisible(false);

    }




}
