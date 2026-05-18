using UnityEngine;
using Mirror;
public class PlayGameButtonUI : MonoBehaviour
{
    [SerializeField] public GameObject PlayGameButton;
    void Update()
    {
        if (NetworkClient.localPlayer != null)
        {
            PlayerController localPC = NetworkClient.localPlayer.GetComponent<PlayerController>();
            if (localPC != null)
            {
                PlayGameButton.SetActive(localPC.isRoomMaster);
            }
            this.enabled = false;
        }
    }
    public void OnClick()
    {
        MatchSetupSystem.Instance.CmdStartGame();
    }
}
