using System;
using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

public class ToolCardSystem : NetworkBehaviour
{
    public override void OnStartServer()
    {
        ActionSystem.AttachPerformer<HailOfArrowsGA>(HailOfArrowsPerformer);
        ActionSystem.AttachPerformer<BarbariansGA>(BarbariansPerformer);
    }
    public override void OnStopServer()
    {
        ActionSystem.DetachPerformer<HailOfArrowsGA>();
        ActionSystem.DetachPerformer<BarbariansGA>();
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
        yield return null;
    }
}