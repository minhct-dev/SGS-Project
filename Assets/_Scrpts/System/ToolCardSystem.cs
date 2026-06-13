using System;
using System.Collections;
using System.Linq;
using Mirror;
using Mono.CompilerServices.SymbolWriter;
using UnityEngine;


public class ToolCardSystem : NetworkBehaviour
{
    public override void OnStartServer()
    {
        ActionSystem.AttachPerformer<HailOfArrowsGA>(HailOfArrowsPerformer);
        ActionSystem.AttachPerformer<BarbariansGA>(BarbariansPerformer);
        ActionSystem.AttachPerformer<BreakGA>(BreakPerformer);
    }
    public override void OnStopServer()
    {
        ActionSystem.DetachPerformer<HailOfArrowsGA>();
        ActionSystem.DetachPerformer<BarbariansGA>();
        ActionSystem.DetachPerformer<BreakGA>();
    }
    private IEnumerator HailOfArrowsPerformer(HailOfArrowsGA hailOfArrowsGA)
    {
        PlayerController[] targets = MatchSetupSystem.Instance.getAllPlayers();
        foreach (PlayerController target in targets)
        {
            if (target == hailOfArrowsGA.User) continue;
            if (target.IsDead()) continue;

            Debug.Log($"Vạn Tiễn bay tới {target.name}. Đợi mục tiêu trả lời...");
            AskForCardGA askForCardGA = new(target, "Basic_Dodge", 1, 10f);
            yield return HealthSystem.Instance.AskForCardPerformer(askForCardGA);
            if (target.isPlayedCard)
            {
                continue;
            }
            target.currentHP -= 1;
        }
        CardSystem.Instance.RpcClearPlayView();
    }
    private IEnumerator BarbariansPerformer(BarbariansGA barbariansGA)
    {
        PlayerController[] targets = MatchSetupSystem.Instance.getAllPlayers();
        foreach (PlayerController target in targets)
        {
            if (target == barbariansGA.User) continue;
            if (target.IsDead()) continue;

            Debug.Log($"Nam man bay tới {target.name}. Đợi mục tiêu trả lời...");
            AskForCardGA askForCardGA = new(target, "Basic_Slash", 1, 10f);
            yield return HealthSystem.Instance.AskForCardPerformer(askForCardGA);
            if (target.isPlayedCard)
            {
                continue;
            }
            target.currentHP -= 1;
        }
        CardSystem.Instance.RpcClearPlayView();
    }

    private IEnumerator BreakPerformer(BreakGA breakGA)
    {
        PlayerController user = breakGA.User;
        PlayerController target = breakGA.Target;
        Debug.Log($"[Server] {user.name} chuẩn bị phá 1 lá bài của {target.name}");
        int targetHandCount = target.currentHand.Count;
        if (targetHandCount == 0) yield break;
        user.isSelecting = true;
        TargetOpenInventoryResponser(user.connectionToClient, target);
        float timeout = 15f;
        while (user.isSelecting && timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
        if (user.isSelecting)
        {
            user.isSelecting = false;

            // Chọn ngẫu nhiên ưu tiên bài trên tay
            if (targetHandCount > 0)
            {
                user.chosenInteractData = new CardInteractData { Area = CardArea.Hand, HandIndex = UnityEngine.Random.Range(0, targetHandCount), CardId = "" };
            }
            // Bắt buộc Client tự đóng UI vì Server đã tự động chọn thay
            TargetForceCloseInventory(user.connectionToClient);
        }
        //-------------------
        CardInteractData choice = user.chosenInteractData;
        if (choice.Area == CardArea.Hand)
        {
            if (choice.HandIndex >= 0 && choice.HandIndex < target.currentHand.Count)
            {
                target.currentHand.RemoveAt(choice.HandIndex);
            }
        }
        yield return null;
    }
    [TargetRpc]
    public void TargetOpenInventoryResponser(NetworkConnection conn, PlayerController target)
    {
        TargetInventoryUI.Instance.OpenPanel(target);
    }
    [TargetRpc]
    public void TargetForceCloseInventory(NetworkConnection conn)
    {
        if (TargetInventoryUI.Instance != null)
        {
            TargetInventoryUI.Instance.ForceClosePanel();
        }
    }
}