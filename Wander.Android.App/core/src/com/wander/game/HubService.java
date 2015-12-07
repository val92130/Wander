package com.wander.game;

import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.transport.ServerSentEventsTransport;

import java.util.concurrent.ExecutionException;

/**
 * Created by valentin on 07/12/2015.
 */
public class HubService {
    HubProxy hub;
    HubConnection connection;

    public HubService(String host, String hubName)
    {
        connection = new HubConnection(host);
        hub = connection.createHubProxy(hubName);

    }

    public void start()
    {
        SignalRFuture<Void> con  =connection.start(new ServerSentEventsTransport(connection.getLogger())); //Or LongPollingTransport
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

    public void connect(){
        UserModel u = new UserModel();
        u.Login = "val_92_";
        u.Email = "val92@live.fr";
        u.Password = "1234";
        u.Sex = 0;
        hub.invoke("Connect", u);
    }
}
