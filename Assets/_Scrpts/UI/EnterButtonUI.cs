using Mirror.Examples.Basic;
using UnityEngine;

public class EnterButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        EnemyTurnGA enemyTurnGA = new();
        ActionSystem.Instance.Perform(enemyTurnGA);
        if (PlayerController.localPlayer != null)
        {
            PlayerController.localPlayer.CmdEndTurn();
        }
    }
}
