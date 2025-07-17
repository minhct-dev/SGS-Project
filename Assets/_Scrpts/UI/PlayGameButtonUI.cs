using UnityEngine;

public class PlayGameButtonUI : MonoBehaviour
{
    [SerializeField] public MatchSetupSystem matchSetupSystem;
    public void OnClick()
    {
        matchSetupSystem.CmdStartGame();
    }
}
