using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
[Serializable]
public class PlayerController : NetworkBehaviour
{
    [Header("Player Info")]
    [SyncVar(hook = nameof(UpdatePlayerName))] public string username;
    // SyncVar hook to call a command whenever a username changes (like when players load in initially).

    //[Header("Portrait")]
    //public Sprite portrait; //For the player's icon at the top left of the screen & in the PartyHUD.(now it for nothing tho)

    [Header("Deck and Hand")]
    public List<CardInstance> hand;

    [Header("Stats")]
    [SyncVar] public int maxHP = 10;
    [SyncVar] public int currentHP = 0;

    [SyncVar] public PlayerType playerType;

    // Quicker access for UI scripts
    [HideInInspector] public static PlayerController localPlayer;
    [HideInInspector] public bool hasOpponent = false;
    [HideInInspector] public static bool isLocalPlayer = false;

    //[HideInInspector] public PlayerInfo opponentInfo; // We can't pass a Player class through the Network, but we can pass structs. 
    // We store all our enemy's info in a PlayerInfo struct so we can pass it through the network when needed.

    // [HideInInspector] public static GameManager gameManager;
    [SyncVar, HideInInspector] public bool firstPlayer = false;
    //overide from networkbehavior
    private void Start()
    {
        //popup for player choose card 

    }

    public void Update()
    {

        // Get EnemyInfo as soon as another player connects. Only start updating once our Player has been loaded in properly (username will be set if loaded in).
        if (!hasOpponent && username != "")
        {
            UpdateEnemyInfo();
        }
        // start game by G button on keyboard
        // if (Input.GetKeyDown(KeyCode.G) && isLocalPlayer)
        // {
        //     gameManager.StartGame();
        // }
    }
    public override void OnStartLocalPlayer()
    {
        localPlayer = this;
        localPlayer.playerType = PlayerType.LOCAL;
        // Get and update the player's username and stats
        CmdLoadPlayer(PlayerPrefs.GetString("Name"));

    }


    //---------------------
    public void UpdatePlayerName(string oldUser, string newUser)
    {
        // Update username
        username = newUser;
        // Update game object's name in editor (only useful for debugging).
        gameObject.name = username;
    }

    [Command]
    public void CmdLoadPlayer(string user)
    {
        // Update the player's username, which calls a SyncVar hook.
        // Learn more here : https://mirror-networking.com/docs/Guides/Sync/SyncVarHook.html
        username = user;
    }
    
    public void UpdateEnemyInfo()
    {
        // Find all Players and add them to the list.
        PlayerController[] onlinePlayers = FindObjectsOfType<PlayerController>();

        // Loop through all online Players (should just be one other Player)
        foreach (PlayerController players in onlinePlayers)
        {
            // Make sure the players are loaded properly (we load the usernames first)
            if (players.username != "")
            {
                // There should only be one other Player online, so if it's not us then it's the enemy.
                if (players != this)
                {
                    //Get & Set PlayerInfo from our Enemy's gameObject
                    //PlayerInfo currentPlayer = new PlayerInfo(players.gameObject);
                    //enemyInfo = currentPlayer;
                    hasOpponent = true;
                    //enemyInfo.data.casterType = Target.OPPONENT;
                    //Debug.LogError("Player " + username + " Enemy " + enemy.username + " / " + enemyInfo.username); // Used for Debugging
                }
            }
        }
    }
    
    public bool IsDead() => currentHP <= 0;
    //public bool CanAttack() => Player.gameManager.isOurTurn && waitTurn == 0 && casterType == Target.FRIENDLIES; (extension for future)
    //public bool CantAttack() => Player.gameManager.isOurTurn && waitTurn > 0 && casterType == Target.FRIENDLIES; (extension for future)
}
