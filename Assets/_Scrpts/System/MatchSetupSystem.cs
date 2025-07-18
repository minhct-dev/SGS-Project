using System;
using System.Collections.Generic;
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
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            //deckSystem.DrawCardPerform(player, 4);
            DrawCardGA drawCardGA = new(player, 4);
            ActionSystem.Instance.Perform(drawCardGA);
        }
        foreach (PlayerController player in players)
        {
            Debug.Log("number of card " + player.currentHand.Count);
            foreach (var card in player.currentHand)
            {
                Debug.Log(card.Number + " " + card.Suit);
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        RpcStartGame();
    }
    [ClientRpc]
    public void RpcStartGame()
    {
        StartGameButtonUI.SetActive(false);
        PlayCardButtonUI.SetActive(true);
        EndTurnButtonUI.SetActive(true);
        StartGame();

    }
}
