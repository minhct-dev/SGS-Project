using System;
using System.Collections.Generic;
using UnityEngine;

public class InputTargetingSystem : Singleton<InputTargetingSystem>
{
    public InputState currentState = InputState.Idle;
    private CardView selectedCard;
    private List<uint> selectedTargetNetIds = new List<uint>();
    private int requiredTarget = 0;

    public void OnCardClicked(CardView cardView)
    {
        if (TurnManagerSystem.Instance.currentPhase != TurnPhase.Play) return;
        //If click a selected card again
        if (selectedCard == cardView)
        {
            //Cancel Selection
            return;
        }
        selectedCard = cardView;
        selectedTargetNetIds.Clear();
        requiredTarget = selectedCard.Card.Data.RequiredTarget;
        if (requiredTarget == 0)
        {

        }
        else
        {
            currentState = InputState.WaitingForTargets;
            Debug.Log($"Waiting for choosing {this.requiredTarget} target for [{this.selectedCard.Card.Name} card]!");
        }
    }
    public void CancelSelection()
    {
        selectedCard = null;
        selectedTargetNetIds.Clear();
        currentState = InputState.Idle;
    }
}