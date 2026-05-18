using Mirror.Examples.Basic;
using UnityEngine;

public class EndturnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        if (PlayerController.localPlayer.isWaitingForServer)
        {
            Debug.Log("Server đang bận xử lý, không thể qua hiệp lúc này!");
            return;
        }
        if (PlayerController.localPlayer != null)
        {
            PlayerController.localPlayer.CmdEndTurn();
        }
    }
}
