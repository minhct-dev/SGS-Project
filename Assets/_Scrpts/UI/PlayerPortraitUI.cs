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
    private PlayerInfo enemyInfo;
    void Update()
    {
        PlayerController player = PlayerController.localPlayer;
        //if (player && player.hasEnemy) enemyInfo = player.enemyInfo;
        
    }
}
