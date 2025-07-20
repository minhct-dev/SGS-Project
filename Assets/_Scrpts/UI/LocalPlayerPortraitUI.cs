using Microsoft.Unity.VisualStudio.Editor;
using Mirror.Examples.Basic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerPortraitUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image CommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image DeputyCommanderPortrait;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text PlayerName;

    //[field: SerializeField] private GameObject wrapper { get; set; }
    private PlayerInfo enemyInfo;
    void Update()
    {
        PlayerController player = PlayerController.localPlayer;
        if (player && player.isLocalPlayer)
        {
            //wrapper.SetActive(true);
            //Debug.Log("connect to local success , Name:" + player.name + " , maxHP =" + player.maxHP +", currHP ="+player.currentHP);
            PlayerName.text = player.name;
            HP.text = player.currentHP.ToString();
        }
        //if (player && player.hasEnemy) enemyInfo = player.enemyInfo;

    }
}
