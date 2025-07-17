using Microsoft.Unity.VisualStudio.Editor;
using Mirror.Examples.Basic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortraitUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image CommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image DeputyCommanderPortrait;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text PlayerName;

    [field: SerializeField] private GameObject wrapper { get; set; }
    private PlayerInfo enemyInfo;
    void Update()
    {
        PlayerController player = PlayerController.localPlayer;
        if (player && player.playerType == PlayerType.LOCAL)
        {
            //Debug.Log("connect to local success , Name:" + player.name + " , maxHP =" + player.maxHP +", currHP ="+player.currentHP);
            PlayerName.text = player.name;
            HP.text = player.currentHP.ToString();  
        }
        else if (player && player.hasOpponent && player.playerType == PlayerType.OTHER)
        {
            wrapper.SetActive(true);
            PlayerName.text = player.name;
            HP.text = player.currentHP.ToString();
        }
        //if (player && player.hasEnemy) enemyInfo = player.enemyInfo;

    }
}
