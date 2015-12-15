package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Dialog;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.wander.game.dialogs.ChangeJobDialog;
import com.wander.game.dialogs.ChatDialog;
import com.wander.game.dialogs.HouseBuyDialog;
import com.wander.game.models.JobModel;
import com.wander.game.models.ServerPropertyModel;

import java.util.ArrayList;

import microsoft.aspnet.signalr.client.Action;

/**
 * Created by val on 13/12/2015.
 */
public class ModalManager {
    private MainGame game;
    private Dialog currentDialogWindow;
    private Stage stage;
    private Skin skin;

    public ModalManager(MainGame game, Stage stage){

        this.game = game;
        this.stage = stage;
        this.skin = AssetManager.getSkin();
    }

    public void openChangeJobDialog(ArrayList<JobModel> dataList)
    {
        if(this.currentDialogWindow == null )
        {
            this.currentDialogWindow = new ChangeJobDialog(this.game, this.stage, this.skin, dataList);
        } else if(!this.currentDialogWindow.isVisible())
        {
            this.currentDialogWindow = new ChangeJobDialog(this.game, this.stage, this.skin, dataList);
        }
    }

    public void openPublicChatBox(){

        if(this.currentDialogWindow == null )
        {
            this.currentDialogWindow = new ChatDialog(this.game, this.stage, this.skin, "Send a public message");
        } else if(!this.currentDialogWindow.isVisible())
        {
            this.currentDialogWindow = new ChatDialog(this.game, this.stage, this.skin, "Send a public message");
        }

    }

    public void openPrivateChatBox(String dest)
    {
        if(this.currentDialogWindow == null )
        {
            this.currentDialogWindow = new ChatDialog(this.game, this.stage, this.skin, "Send a message to " + dest, dest);
        } else if(!this.currentDialogWindow.isVisible())
        {
            this.currentDialogWindow = new ChatDialog(this.game, this.stage, this.skin, "Send a message to " + dest, dest);
        }


    }

    public void openBuyPropertyModal(int propertyId)
    {
        this.game.getHubService().getHub().invoke(ServerPropertyModel.class, "GetPropertyInfo", propertyId).done(new Action<ServerPropertyModel>() {
            @Override
            public void run(ServerPropertyModel serverPropertyModel) throws Exception {
                final ServerPropertyModel _res = serverPropertyModel;
                Gdx.app.postRunnable(new Runnable() {
                    @Override
                    public void run() {
                        System.out.println(_res);
                        openModalProperty(_res);
                    }
                });
            }
        });
    }

    private void openModalProperty(ServerPropertyModel model)
    {
        if(model != null)
        {
            if(this.currentDialogWindow == null )
            {
                this.currentDialogWindow = new HouseBuyDialog(this.game, this.stage, this.skin, "Buy property : " + model.PropertyName + "?", model);
            } else if(!this.currentDialogWindow.isVisible())
            {
                this.currentDialogWindow = new HouseBuyDialog(this.game, this.stage, this.skin, "Buy property : " + model.PropertyName + "?", model);
            }

        }
    }
}
