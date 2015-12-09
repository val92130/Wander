package com.wander.game.services;

import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;

/**
 * Created by valentin on 09/12/2015.
 */
public interface IHubService {
    void start();
    boolean connect(String pseudo, String password);
    public HubProxy getHub();
    public HubConnection getConnection();


}
