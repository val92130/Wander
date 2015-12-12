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

    private Stage stage;
    private Date now;
    private Skin skin;
    private BitmapFont font;
    private Actor currentNotification;
    private boolean finished = false;

    public NotificationManager(MainGame game)
    {
        this.stage = new Stage();
        this.skin = new Skin(Gdx.files.internal("uiskin.json"));
        this.font = this.skin.getFont("default-font");
    }

    public void add(String content, EMessageType type)
    {
        Vector2 pos = new Vector2(0, Gdx.graphics.getHeight() - (util.GetStringHeight(this.font, content) + (30 * 2)));

        TextButton notif = createNotification(pos,content,type);
        currentNotification = notif;

        if(stage.getActors().size >= 1 ){
            stage.getActors().get(0).remove();
        }

        stage.addActor(notif);
        now = new Date();
        this.finished = false;
    }


    public void update(){
        this.stage.act();

        if(now != null)
        {
            Date t = new Date();
            long diffInMilli = (t.getTime() - now.getTime()) ;
            if(diffInMilli >= Constants.NOTIFICATION_DURATION_MILLI){
                finished = true;
            }

            if(finished)
            {
                if(currentNotification != null)
                {
                    if(stage.getActors().size >= 1 ){
                        Color c = new Color(stage.getActors().get(0).getColor().r,stage.getActors().get(0).getColor().g,stage.getActors().get(0).getColor().b,stage.getActors().get(0).getColor().a - 0.05f);
                        stage.getActors().get(0).setColor(c);
                        if(c.a <= 0)
                        {
                            stage.getActors().get(0).remove();
                            this.currentNotification = null;
                            this.finished = false;
                            this.now = new Date();
                        }

                    }
                }
            }
        }

    }

    private TextButton createNotification(Vector2 position, String content, EMessageType type)
    {
        TextButton notifButton = new TextButton(content, new Skin(Gdx.files.internal("uiskin.json")), "default");
        Color c;
        switch(type){
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
        float btnWidth = util.GetStringWidth(this.font, content);
        float btnHeight = util.GetStringHeight(this.font, content);

        notifButton.setWidth(btnWidth + padding * 2);
        notifButton.setHeight(btnHeight + padding * 2);
        notifButton.setPosition(Gdx.graphics.getWidth() / 2 - notifButton.getWidth()/2, position.y + padding / 2);

        return notifButton;
    }

    public void render(SpriteBatch batch){
        this.stage.draw();
    }
}
