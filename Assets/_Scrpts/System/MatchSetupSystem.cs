using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

public class MatchSetupSystem : NetworkBehaviour
{
    public static MatchSetupSystem Instance;
    [SerializeField] private GameObject PlayCardButtonUI;
    [SerializeField] private GameObject EndTurnButtonUI;
    [SerializeField] private GameObject StartGameButtonUI;

    [SerializeField] private DeckSystem deckSystem;
    public PlayerController[] allPlayers;
    private void Awake() => Instance = this;
    [Server]
    public void StartGame()
    {
        deckSystem.BuildFullDeck();
        allPlayers = Object.FindObjectsByType<PlayerController>();
        Debug.Log("Number of client working:" + allPlayers.Length);
        Debug.Log("Players List: " + string.Join(", ", allPlayers.Select(players => players.name)));
        foreach (PlayerController player in allPlayers)
        {
            Debug.Log("Player :" + player.name + "is room master: " + player.isRoomMaster);
        }   //debug perpose 
        GameAction setupGameAction = new SetupGameGA();
        //Start turn
        foreach (PlayerController p in allPlayers)
        {
            DrawCardGA drawCardGA = new DrawCardGA(p, 4);
            setupGameAction.PerformReactions.Add(drawCardGA);

        }
        //Đây là giải pháp tạm thời và nó rất ngu @@
        ActionSystem.Instance.Perform(setupGameAction, () => TurnManagerSystem.Instance.Initialized(allPlayers));
        //TurnManagerSystem.Instance.Initialized(players);


    }

    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        StartGame();
        RpcStartGameUI();
    }
    [ClientRpc]
    public void RpcStartGameUI()
    {
        StartGameButtonUI.SetActive(false);
        PlayCardButtonUI.SetActive(true);
        EndTurnButtonUI.SetActive(true);
    }
    public PlayerController[] getAllPlayers() => allPlayers;
}
