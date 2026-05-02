using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class MatchSetupSystem : NetworkBehaviour
{
    [SerializeField] private GameObject PlayCardButtonUI;
    [SerializeField] private GameObject EndTurnButtonUI;
    [SerializeField] private GameObject StartGameButtonUI;

    [SerializeField] private DeckSystem deckSystem;
    [Server]
    public void StartGame()
    {
        deckSystem.BuildFullDeck();
        PlayerController[] players = FindObjectsOfType<PlayerController>().Reverse().ToArray();
        Debug.Log("Number of client working:" + players.Length);
        Debug.Log("Players List: " + string.Join(", ", players.Select(players => players.name)));
        foreach (PlayerController player in players)
        {
            Debug.Log("Player :" + player.name + "is room master: " + player.isRoomMaster);
        }   //debug perpose 
        GameAction setupGameAction = new SetupGameGA();
        //Start turn
        foreach (PlayerController p in players)
        {
            DrawCardGA drawCardGA = new DrawCardGA(p, 4);
            deckSystem.DrawCardLogicGA(drawCardGA);
            setupGameAction.PerformReactions.Add(drawCardGA);

        }
        ActionSystem.Instance.Perform(setupGameAction, () => TurnManagerSystem.Instance.Initialized(players));



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
}
