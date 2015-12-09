package com.wander.game.services;

import com.wander.game.models.UserModel;

import java.util.concurrent.ExecutionException;

import microsoft.aspnet.signalr.client.Action;
import microsoft.aspnet.signalr.client.LogLevel;
import microsoft.aspnet.signalr.client.Logger;
import microsoft.aspnet.signalr.client.Platform;
import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.http.android.AndroidPlatformComponent;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.transport.ServerSentEventsTransport;

/**
 * Created by valentin on 09/12/2015.
 */
public class AndroidHubService implements IHubService {
    private HubProxy hub;
    private HubConnection connection;
    private boolean successConnect;

    public AndroidHubService(String host, String hubName)
    {
        Platform.loadPlatformComponent(new AndroidPlatformComponent());
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

    }

    public void start()
    {
        SignalRFuture<Void> con=connection.start(new ServerSentEventsTransport(connection.getLogger())); //Or LongPollingTransport
        System.out.println("Connecting");
        try {
            con.get();
            System.out.println("Connected");
        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch (ExecutionException e) {
            e.printStackTrace();
        }

    }

    public boolean connect(String pseudo, String password){
        successConnect = false;
        if(pseudo == null || password == null) try {
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
            hub.invoke(Boolean.class,"Connect", u).done(new Action<Boolean>() {
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
        return successConnect;
    }

    public HubProxy getHub() {
        return hub;
    }

    public HubConnection getConnection() {
        return connection;
    }
}
