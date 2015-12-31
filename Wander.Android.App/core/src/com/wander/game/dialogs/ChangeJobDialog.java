package com.wander.game.dialogs;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Dialog;
import com.badlogic.gdx.scenes.scene2d.ui.Label;
import com.badlogic.gdx.scenes.scene2d.ui.SelectBox;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.badlogic.gdx.utils.Align;
import com.badlogic.gdx.utils.Array;
import com.wander.game.AssetManager;
import com.wander.game.MainGame;
import com.wander.game.models.JobModel;

import java.util.ArrayList;


/**
 * Created by valentin on 15/12/2015.
 */
public class ChangeJobDialog extends Dialog {

    private MainGame game;
    private Stage stage;
    private SelectBox<JobModel> selectBox;


    public ChangeJobDialog(MainGame game, Stage stage, Skin skin, ArrayList<JobModel> dataList)
    {
        super("", skin);
        this.stage = stage;
        this.game = game;

        this.setWidth(Gdx.graphics.getWidth() * 0.8f);

        this.getContentTable().row();
        this.padTop(50).padBottom(50);

        this.getButtonTable().padTop(50);
        this.getContentTable().row();


        Skin z = AssetManager.getSkin();
        BitmapFont b = z.getFont("default-font");
        b.getData().setScale(0.5f,.5f);
        this.selectBox = new SelectBox<JobModel>(z);

        Array<JobModel> arr = new Array<JobModel>(dataList.size());
        for(int i = 0; i < dataList.size(); i++){
            arr.add(dataList.get(i));
        }
        this.selectBox.setItems(arr);

        this.getContentTable().add(this.selectBox).fillX().center();


        TextButton dbutton = new TextButton("Apply to job", skin);
        this.button(dbutton, true);

        this.getContentTable().center();

        this.key(Input.Keys.ENTER, true).key(Input.Keys.ESCAPE, false);
        this.invalidateHierarchy();
        this.invalidate();
        this.layout();
        this.show(stage);
    }

    @Override
    protected void result (Object object) {
        System.out.println("Applying ! " + this.selectBox.getSelected());
        if(this.selectBox.getSelected() != null)game.changeJob(this.selectBox.getSelected());

        this.setVisible(false);
    }
}
