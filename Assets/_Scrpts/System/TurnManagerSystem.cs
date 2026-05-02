using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class TurnManagerSystem : NetworkBehaviour
{
    public static TurnManagerSystem Instance;
    [SyncVar(hook = nameof(OnTurnChanged))]
    public int currentPlayerIndex = -1;
    private readonly SyncList<PlayerController> players = new SyncList<PlayerController>();
    private void Awake() => Instance = this;

    [SerializeField] GameObject playCardButtonUI;
    [SerializeField] GameObject endTurnButtonUI;


    [Server]
    public void Initialized(PlayerController[] allPlayer)
    {
        players.Clear();
        foreach (var p in allPlayer)
        {
            players.Add(p);
        }
        NextTurn();
    }
    [Server]
    public void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count();
        StartCoroutine(ExecuteTurnPhase(players[currentPlayerIndex]));
    }
    //Function to execute turnphase in SGS
    /*
    Phase 1: judgment
    Phase 2: draw card
    Phase 3: play card
    Phase 4: discard
    phase 5: end turn 
    
    */
    [Server]
    public IEnumerator ExecuteTurnPhase(PlayerController ActivePlayer)
    {
        Debug.Log($"Player {ActivePlayer.name} start turn");
        //Phase 1

        //Phase 2: Draw Card
        // DrawCardGA drawPhaseGA = new DrawCardGA(ActivePlayer, 2);
        // ActionSystem.Instance.Perform(drawPhaseGA);
        //phase 3 

        //Phase 4

        //Phase 5
        yield return null;
    }
    //hook run when turn changed 
    private void OnTurnChanged(int oldIndex, int newIndex)
    {
        if (newIndex >= 0 && newIndex < players.Count)
        {
            UpdateUI(newIndex);
        }

    }
    private void UpdateUI(int activeIndex)
    {
        bool isMyTurn = NetworkClient.localPlayer.GetComponent<PlayerController>() == players[activeIndex];
        Debug.Log("is my turn: " + isMyTurn);
        if (isMyTurn)
        {
            //turn on/off UI of play button 
            playCardButtonUI.SetActive(true);
            endTurnButtonUI.SetActive(true);

        }
        else
        {
            playCardButtonUI.SetActive(false);
            endTurnButtonUI.SetActive(false);
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdEndTurn()
    {
        //Only player in turn can call endturn
        if (players[currentPlayerIndex].connectionToClient != connectionToClient) return;
        NextTurn();
    }
}
