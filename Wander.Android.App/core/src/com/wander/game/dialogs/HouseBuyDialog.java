package com.wander.game.dialogs;

import com.badlogic.gdx.Input;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Dialog;
import com.badlogic.gdx.scenes.scene2d.ui.Label;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.badlogic.gdx.scenes.scene2d.utils.ClickListener;
import com.badlogic.gdx.utils.Align;
import com.wander.game.MainGame;
import com.wander.game.models.ServerPropertyModel;

import java.awt.event.InputEvent;

/**
 * Created by val on 12/12/2015.
 */
public class HouseBuyDialog extends Dialog {
    private MainGame game;
    private Stage stage;
    private ServerPropertyModel propertyModel;

    public HouseBuyDialog(MainGame game, Stage stage, Skin skin,String labelContent, ServerPropertyModel propertyModel) {
        super("", skin);
        this.stage = stage;
        this.game = game;
        this.propertyModel = propertyModel;

        Label label = new Label(labelContent, skin);

        label.setWrap(true);
        label.setAlignment(Align.center);
        this.getContentTable().row();
        this.padTop(50).padBottom(50);
        this.getContentTable().add(label).width(850).padBottom(50).row();

        Label priceLabel = new Label("Price : " + propertyModel.Price, skin);
        priceLabel.setWrap(true);
        priceLabel.setAlignment(Align.center);
        priceLabel.setColor(Color.GREEN);
        this.getContentTable().add(priceLabel).width(850).padBottom(50).row();

        this.getButtonTable().padTop(50);
        this.getContentTable().row();

        TextButton dbutton = new TextButton("Buy", skin);
        this.button(dbutton, true);

        TextButton closeButton = new TextButton("Close", skin);
        final HouseBuyDialog _this = this;
        closeButton.addListener(new ClickListener(){

            @Override
            public void clicked(com.badlogic.gdx.scenes.scene2d.InputEvent event, float x, float y) {
                _this.setVisible(false);
                _this.hide();
            }
        });
        this.getButtonTable().add(closeButton);

        this.getContentTable().center();

        this.key(Input.Keys.ENTER, true).key(Input.Keys.ESCAPE, false);
        this.invalidateHierarchy();
        this.invalidate();
        this.layout();
        this.show(stage);

    }
    @Override
    protected void result (Object object) {
        System.out.println("bought ! " + propertyModel.PropertyDescription);

        try{
            boolean val = ((Boolean) object).booleanValue();
            if(val)
            {
                game.buyProperty(propertyModel);

            }

        } catch(Exception e )
        {

        }
        this.setVisible(false);
    }

}
