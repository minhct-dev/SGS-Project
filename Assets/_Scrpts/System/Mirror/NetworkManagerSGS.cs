using System.Collections.Generic;
using UnityEngine;
using Mirror;


// Doesnt do anything special but it's set up to be built-upon
[AddComponentMenu("Network Manager CCG")]
public class NetworkManagerSGS : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
        PlayerController newPlayer = conn.identity.GetComponent<PlayerController>();
        if (numPlayers == 1)
        {
            newPlayer.isRoomMaster = true;
        }
        else
        {
            newPlayer.isRoomMaster = false;
        }
    }
}
