using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class ToolCardSystem : NetworkBehaviour
{
    public override void OnStartServer()
    {
        ActionSystem.AttachPerformer<HailOfArrowsGA>(HailOfArrowsPerformer);
    }
    public override void OnStopServer()
    {
        ActionSystem.DetachPerformer<HailOfArrowsGA>();
    }
    private IEnumerator HailOfArrowsPerformer(HailOfArrowsGA hailOfArrowsGA)
    {
        PlayerController[] targets = MatchSetupSystem.Instance.getAllPlayers();
        foreach (PlayerController target in targets)
        {
            if (target == hailOfArrowsGA.User) continue;
            if (target.IsDead()) continue;
            Debug.Log($"Vạn Tiễn bay tới {target.name}. Đợi mục tiêu trả lời...");
            yield return target.AskForDodge();
            if (target.isDodgeCardPlayed)
            {
                yield return HealthSystem.Instance.DodgePerformer(new DodgeGA(target, target.playedDodgeCard));
                continue;
            }
            target.currentHP -= 1;
        }
        CardSystem.Instance.RpcClearPlayView();
    }

}