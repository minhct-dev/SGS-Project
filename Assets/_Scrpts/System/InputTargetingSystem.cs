using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputTargetingSystem : Singleton<InputTargetingSystem>
{
    public InputState currentState = InputState.Idle;
    [SerializeField] private CardView selectedCard;
    [SerializeField] private List<uint> selectedTargetNetIds = new List<uint>();
    [SerializeField] private int requiredTarget = 0;
    [SerializeField] private GameObject playCardButtonUI;
    public void OnCardClicked(CardView cardView)
    {
        if (TurnManagerSystem.Instance.currentPhase != TurnPhase.Play) return;
        //If click a selected card again
        if (selectedCard == cardView)
        {
            //Cancel Selection
            CancelSelection();
            return;
        }
        selectedCard = cardView;
        selectedTargetNetIds.Clear();
        requiredTarget = selectedCard.Card.Data.RequiredTarget;
        if (requiredTarget == 0)
        {
            currentState = InputState.Idle;

        }
        else
        {
            currentState = InputState.WaitingForTargets;
            //Cần có code để biểu thị valid target
            Debug.Log($"Waiting for choosing {this.requiredTarget} target for [{this.selectedCard.Card.Name} card]!");
        }
    }
    public void CancelSelection()
    {
        selectedCard = null;
        selectedTargetNetIds.Clear();
        currentState = InputState.Idle;
    }
    public bool IsReadyToPlay()
    {
        if (selectedCard == null) return false;
        return selectedTargetNetIds.Count == requiredTarget;
    }
}