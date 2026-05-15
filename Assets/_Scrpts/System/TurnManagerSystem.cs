using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;
public class TurnManagerSystem : NetworkBehaviour
{
    public static TurnManagerSystem Instance;
    private readonly SyncList<PlayerController> players = new SyncList<PlayerController>();
    [SyncVar(hook = nameof(OnPhaseChanged))]
    public TurnPhase currentPhase;
    [SyncVar(hook = nameof(OnTurnOwnerChanged))]
    public uint activePlayerNetId;
    public int currentPlayerIndex = -1;

    private void Awake() => Instance = this;

    [SerializeField] GameObject playCardButtonUI;
    [SerializeField] GameObject endTurnButtonUI;
    [SerializeField] GameObject playDodgeButtonUI;
    [SerializeField] GameObject CancelButtonUI;

    [Server]
    public void Initialized(PlayerController[] allPlayer)
    {
        players.Clear();
        foreach (var p in allPlayer)
        {
            players.Add(p);
        }
        currentPlayerIndex = -1;
        NextTurn();
    }
    [Server]
    public void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count();
        activePlayerNetId = players[currentPlayerIndex].netId;
        Debug.Log($"[Sever] bắt đầu lượt của {players[currentPlayerIndex].name}");
        ChangePhase(TurnPhase.Start);
    }
    //Function to execute turnphase in SGS
    /*
    Phase 0: Start turn
    Phase 1: judgment
    Phase 2: draw card
    Phase 3: play card
    Phase 4: discard
    phase 5: end turn 
    */
    [Server]
    public void ChangePhase(TurnPhase nextPhase)
    {
        currentPhase = nextPhase;
        Debug.Log($"[Sever] Chuyển sang phase: {currentPhase}");
        PlayerController activePlayer = players[currentPlayerIndex];

        switch (currentPhase)
        {
            case TurnPhase.Start:
                //TODO: Hand skill on start turn 
                ChangePhase(TurnPhase.Judgment);
                break;
            case TurnPhase.Judgment:
                //TODO: Handle lá phán xét : lạc bất tư thục / Xử lý Binh Dục Cửu
                //Nếu dính lạc bất tư thục nhảy sang Discard. Nếu không:
                ChangePhase(TurnPhase.Draw);
                break;
            case TurnPhase.Draw:
                //TODO: Rút 2 lá / xử lý kĩ năng lúc rút bài 
                DrawCardGA drawPhaseGA = new DrawCardGA(activePlayer, 2);
                ActionSystem.Instance.Perform(drawPhaseGA);
                ChangePhase(TurnPhase.Play);
                break;
            case TurnPhase.Play:
                // TODO: Tính toán xem trên tay có bao nhiêu lá, máu bao nhiêu.
                // Nếu bài > Máu -> Yêu cầu Client vứt bài (Đóng băng Server chờ vứt)
                // Nếu bài <= Máu -> Nhảy thẳng sang End
                break;
            case TurnPhase.Discard:
                // TODO: Tính toán xem trên tay có bao nhiêu lá, máu bao nhiêu.
                // Nếu bài > Máu -> Yêu cầu Client vứt bài (Đóng băng Server chờ vứt)
                // Nếu bài <= Máu -> Nhảy thẳng sang End
                ChangePhase(TurnPhase.End);
                break;
            case TurnPhase.End:
                // Kích hoạt kỹ năng cuối lượt (Ví dụ: Bế Nguyệt của Điêu Thuyền)
                NextTurn();
                break;
        }
    }
    //hook run when turn changed 
    private void OnTurnOwnerChanged(uint oldIndex, uint newIndex)
    {
        UpdateUI();
    }
    private void OnPhaseChanged(TurnPhase oldPhase, TurnPhase newPhase)
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (NetworkClient.localPlayer == null) return;
        bool isMyTurn = NetworkClient.localPlayer.netId == activePlayerNetId;
        bool canPlay = isMyTurn && (currentPhase == TurnPhase.Play);
        Debug.Log("is my turn: " + isMyTurn);
        playCardButtonUI.SetActive(canPlay);
        endTurnButtonUI.SetActive(canPlay);
    }
    [Server]
    public void RequestEndTurn(PlayerController playerRequest)
    {
        Debug.Log(playerRequest.name + " request end turn in turn phase: " + currentPhase);
        if (playerRequest.netId != activePlayerNetId) return;
        if (currentPhase != TurnPhase.Play) return;
        ChangePhase(TurnPhase.Discard);
    }
    //Target of slash card ui 
    [TargetRpc]
    public void TargetAskForDodge(NetworkConnection target)
    {
        playDodgeButtonUI.SetActive(true);
        CancelButtonUI.SetActive(true);
    }
}
