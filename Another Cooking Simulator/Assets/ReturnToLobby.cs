﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReturnToLobby : NetworkBehaviour
{
    NetworkRoomManager myNetworkRoomManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    [Server]
    public void BackToLobby()
    {
        myNetworkRoomManager = FindObjectOfType<NetworkRoomManager>();
        myNetworkRoomManager.ServerChangeScene(myNetworkRoomManager.RoomScene);
        RpcBackToLobby();
    }


    [ClientRpc]
    private void RpcBackToLobby()
    {
        Debug.Log("AAAAAAAAA");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
