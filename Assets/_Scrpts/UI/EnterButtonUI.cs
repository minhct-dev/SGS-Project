using Mirror.Examples.Basic;
using UnityEngine;

public class EnterButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        if (PlayerController.localPlayer != null)
        {
            PlayerController.localPlayer.CmdEndTurn();
        }
    }
}
