package com.wander.game;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.scenes.scene2d.ui.Skin;
import com.badlogic.gdx.scenes.scene2d.ui.TextButton;
import com.wander.game.models.EMessageType;

import java.util.ArrayList;
import java.util.Date;

/**
 * Created by valentin on 09/12/2015.
 */
public class NotificationManager {

    private ArrayList<Actor> notificationList;
    private Stage stage;
    private Date now;

    public NotificationManager(MainGame game)
    {
        notificationList = new ArrayList<Actor>();
        this.stage = new Stage();
    }

    public void add(MainGame game, String content, EMessageType type)
    {
        Vector2 pos = new Vector2(0, Gdx.graphics.getHeight() - (util.GetStringHeight(new BitmapFont(), content) + (30 * 2)));
        Notification newNotif = new Notification(game,pos,content,type);
        if(notificationList.size() >= 1)
        {
            Vector2 lastNotifPos = new Vector2(notificationList.get(notificationList.size() -1).getX(),notificationList.get(notificationList.size() -1).getY());
            newNotif.setPosition(new Vector2(newNotif.getPosition().x, lastNotifPos.y - 20 - notificationList.get(notificationList.size() -1).getHeight()));
        }

        TextButton notif = createNotification(newNotif);
        notificationList.add(notif);
        stage.addActor(notif);
        now = new Date();
    }


    public void update(){
        this.stage.act();

        if(now != null)
        {
            Date t = new Date();
            long diffInSeconds = (t.getTime() - now.getTime()) ;

            if(diffInSeconds >= 2000){
                this.now = new Date();
                if(stage.getActors().size >= 1 && this.notificationList.size() >= 1 ){
                    stage.getActors().get(0).remove();
                    this.notificationList.remove(0);
                    System.out.println("Elapsed");
                }

            }
        }

    }

    private TextButton createNotification(Notification notification)
    {
        TextButton notifButton = new TextButton(notification.getContent(), new Skin(Gdx.files.internal("uiskin.json")), "default");
        Color c;
        switch(notification.getMessageType()){
            case info:
                c = new Color(0.10f, 0, 0.85f,0.5f);
                break;
            case error:
                c = new Color(0.95f, 0, 0.05f,0.5f);
                break;
            case warn:
                c = Color.YELLOW;
                break;
            case success:
                c = Color.GREEN;
                break;
            default:
                c = Color.ROYAL;
                break;

        }
        notifButton.setColor(c);
        int padding = 20;
        notifButton.setWidth(notification.getWidth() + padding * 2);
        notifButton.setHeight(notification.getHeight() + padding * 2);
        notifButton.setPosition(notification.getPosition().x + padding/2, notification.getPosition().y + padding / 2);

        return notifButton;
    }

    public void render(SpriteBatch batch){
        this.stage.draw();
    }
}
