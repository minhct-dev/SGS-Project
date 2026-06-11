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
            List<PlayerUI> allPortraits = PlayerPortraitCreator.Instance.SpawnedPortraits;
            foreach (var portrait in allPortraits)
            {
                // Cần có code check logic
                portrait.UpdateTargetableState(true);
            }
            Debug.Log($"Waiting for choosing {this.requiredTarget} target for [{this.selectedCard.Card.Name} card]!");
        }
    }
    public void OnPlayerAvatarClicked(PlayerController targetPlayer)
    {
        if (currentState != InputState.WaitingForTargets) return;
        if (selectedCard == null) return;
        //In case click again the player before
        if (selectedTargetNetIds.Contains(targetPlayer.netId))
        {
            selectedTargetNetIds.Remove(targetPlayer.netId);
        }
        else
        {
            if (selectedTargetNetIds.Count < requiredTarget)
            {
                selectedTargetNetIds.Add(targetPlayer.netId);
                if (selectedTargetNetIds.Count == requiredTarget)
                {
                    Debug.Log("Đã đủ mục tiêu! Hãy bấm nút đánh bài");
                }
            }

        }
    }
    public void CancelSelection()
    {
        selectedCard = null;
        selectedTargetNetIds.Clear();
        currentState = InputState.Idle;
        requiredTarget = 0;
        ResetAllPortraitsUI();
    }
    public bool IsReadyToPlay()
    {
        if (selectedCard == null) return false;
        return selectedTargetNetIds.Count == requiredTarget;
    }
    private void ResetAllPortraitsUI()
    {
        if (PlayerPortraitCreator.Instance == null) return;

        List<PlayerUI> allPortraits = PlayerPortraitCreator.Instance.SpawnedPortraits;
        foreach (var portrait in allPortraits)
        {
            portrait.UpdateTargetableState(false);
        }
    }
    public uint[] GetTargetIds() => selectedTargetNetIds.ToArray();
    public CardView GetSelectedCard() => selectedCard;
}