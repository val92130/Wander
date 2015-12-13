package com.wander.game.services;

import com.badlogic.gdx.Application;
import com.badlogic.gdx.Gdx;
import com.wander.game.MainGame;
import com.wander.game.models.EMessageType;
import com.wander.game.models.NotificationMessage;
import com.wander.game.models.UserModel;

import microsoft.aspnet.signalr.client.Action;
import microsoft.aspnet.signalr.client.ConnectionState;
import microsoft.aspnet.signalr.client.ErrorCallback;
import microsoft.aspnet.signalr.client.LogLevel;
import microsoft.aspnet.signalr.client.Logger;
import microsoft.aspnet.signalr.client.Platform;
import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.http.android.AndroidPlatformComponent;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler2;
import microsoft.aspnet.signalr.client.transport.ServerSentEventsTransport;

import java.util.concurrent.ExecutionException;

/**
 * Created by valentin on 07/12/2015.
 */
public class HubService implements IHubService {


    private HubProxy hub;
    private HubConnection connection;
    private boolean successConnect;
    private MainGame game;

    public HubService(MainGame game, String host, String hubName)
    {
        if(Gdx.app.getType() == Application.ApplicationType.Android) Platform.loadPlatformComponent(new AndroidPlatformComponent());

        this.game = game;
        Logger logger = new Logger() {

            @Override
            public void log(String message, LogLevel level) {

                if(level == LogLevel.Critical)
                {
                    System.out.println(message);

                }
            }
        };

        connection = new HubConnection(host, "", true, logger);
        hub = connection.createHubProxy(hubName);
        connection.error(new ErrorCallback() {

            @Override
            public void onError(Throwable error) {
                System.err.println("There was an error communicating with the server.");
                System.err.println("Error detail: " + error.toString());
                connectionError();
                error.printStackTrace(System.err);
            }
        });



    }

    private void connectionError()
    {

        this.game.onConnectionError();
    }

    public void start()
    {
        SignalRFuture<Void> con=connection.start(new ServerSentEventsTransport(connection.getLogger())); //Or LongPollingTransport
        System.out.println("Connecting");
        try {
            con.get();
            game.onConnectionEstablished();
            System.out.println("Connected");
        } catch (InterruptedException e) {
            e.printStackTrace();
            System.out.println("Connection interrupted");
            this.game.onConnectionError();
        } catch (ExecutionException e) {
            this.game.onConnectionError();
            e.printStackTrace();
        }
    }

    public boolean connect(String pseudo, String password){

        successConnect = false;
        if(this.connection.getState() != ConnectionState.Connected){
            this.start();
            this.game.addNotification("Connection error, check your network", EMessageType.error);
        } else{
            if (pseudo == null || password == null) try {
                throw new Exception("Pseudo or password is null");
            } catch (Exception e) {
                e.printStackTrace();
            }
            UserModel u = new UserModel();
            u.Login = pseudo;
            u.Email = "mail";
            u.Password = password;
            u.Sex = 0;
            try {
                hub.invoke(Boolean.class, "Connect", u).done(new Action<Boolean>() {
                    @Override
                    public void run(Boolean val) throws Exception {
                        successConnect = val;
                    }
                }).get();
            } catch (InterruptedException e) {
                e.printStackTrace();
            } catch (ExecutionException e) {
                e.printStackTrace();
            }
        }
        return successConnect;
    }

    public HubProxy getHub() {
        return hub;
    }

    public HubConnection getConnection() {
        return connection;
    }
}
