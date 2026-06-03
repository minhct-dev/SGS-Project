using Microsoft.Unity.VisualStudio.Editor;
using Mirror;
using Mirror.Examples.Basic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerUI : NetworkBehaviour
{
    [SerializeField] private UnityEngine.UI.Image CommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image DeputyCommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image AvatarPanel;
    [SerializeField] private UnityEngine.UI.Image CountryName;
    [SerializeField] private TMP_Text PlayerName;
    private PlayerController localPlayer;
    //[field: SerializeField] private GameObject wrapper { get; set; }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        localPlayer = PlayerController.localPlayer;
    }
    void Update()
    {
        if (localPlayer == null) return;
        //wrapper.SetActive(true);
        //Debug.Log("connect to local success , Name:" + player.name + " , maxHP =" + player.maxHP +", currHP ="+player.currentHP);
        PlayerName.text = localPlayer.name;
        //if (player && player.hasEnemy) enemyInfo = player.enemyInfo;
    }
}
