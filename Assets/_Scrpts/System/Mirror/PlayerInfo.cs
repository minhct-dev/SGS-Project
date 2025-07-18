using Mirror;
using UnityEngine;

public partial struct PlayerInfo
{
    public GameObject player;
    public PlayerInfo(GameObject player)
    {
        this.player = player;
    }
    public PlayerController data
    {
        get
        {
            // Return ScriptableItem from our cached list, based on the card's uniqueID.
            return player.GetComponentInChildren<PlayerController>();
        }
    }

    // Player's username
    public string username => data.username;
    //public Sprite portrait => data.portrait;

    //player HP and max HP
    public int currentHP => data.currentHP;
    public int maxHP => data.maxHP;

    // Card count for UI
    public int handCount => data.currentHand.Count;
}
public class SyncListPlayerInfo : SyncList<PlayerInfo> { }