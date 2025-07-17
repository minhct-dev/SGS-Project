using Mirror;
using Newtonsoft.Json;
using UnityEngine;

public class MatchSetupSystem : NetworkBehaviour
{
    [SerializeField] private GameObject PlayCardButtonUI;
    [SerializeField] private GameObject EndTurnButtonUI;
    [SerializeField] private GameObject StartGameButtonUI;

    public void StartGame()
    {
        
        //     CardSystem.Instance.Setup();
        //     DrawCardGA drawCardGA = new(4);
        //     ActionSystem.Instance.Perform(drawCardGA);
    }

    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        RpcStartGame();
    }
    [ClientRpc]
    public void RpcStartGame()
    { 
        StartGameButtonUI.SetActive(false);
        PlayCardButtonUI.SetActive(true);
        EndTurnButtonUI.SetActive(true);
    }
}
