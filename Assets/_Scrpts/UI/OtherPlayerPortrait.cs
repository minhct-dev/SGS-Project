using System.Globalization;
using TMPro;
using UnityEngine;

public class OtherPlayerPortrait : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image CommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image DeputyCommanderPortrait;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text PlayerName;
    [SerializeField] private TMP_Text NumberOfCards;
    [SerializeField] private GameObject wrapper;
    public PlayerController assignPlayer { get; set; } = null;

    // Update is called once per frame
    void Update()
    {
        if (assignPlayer != null && !assignPlayer.isLocalPlayer)
        {
            PlayerName.text = assignPlayer.name;
            HP.text = assignPlayer.currentHP.ToString();
            NumberOfCards.text = assignPlayer.currentHand.Count.ToString();
        }
    }
}
