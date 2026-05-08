using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class OtherPlayerPortrait : MonoBehaviour
{
    [SerializeField] private Image CommanderPortrait;
    [SerializeField] private Image DeputyCommanderPortrait;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text PlayerName;
    [SerializeField] private TMP_Text NumberOfCards;
    [SerializeField] private GameObject wrapper;
    [Header("Targeting Visuals")]
    [SerializeField] private Outline targetOutline;

    public PlayerController assignPlayer { get; set; } = null;

    // Update is called once per frame
    void Update()
    {
        if (assignPlayer != null && !assignPlayer.isLocalPlayer)
        {
            PlayerName.text = assignPlayer.name;
            HP.text = assignPlayer.currentHP.ToString();
            NumberOfCards.text = assignPlayer.currentHand.Count.ToString();
            UpdateBorderVisual();
        }
    }

    public virtual void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (assignPlayer == null) return;
        if (InputTargetingSystem.Instance != null)
        {
            InputTargetingSystem.Instance.OnPlayerAvatarClicked(assignPlayer);
        }
    }

    private void UpdateBorderVisual()
    {
        uint[] currentTargets = InputTargetingSystem.Instance.GetTargetIds();
        bool isSelected = currentTargets != null && currentTargets.Contains(assignPlayer.netId);
        if (targetOutline != null)
        {
            targetOutline.enabled = isSelected;
            targetOutline.effectColor = Color.red;
        }
    }

}
