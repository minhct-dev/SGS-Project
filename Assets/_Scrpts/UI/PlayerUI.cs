using Microsoft.Unity.VisualStudio.Editor;
using Mirror;
using Mirror.Examples.Basic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image CommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image DeputyCommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image AvatarPanel;
    [SerializeField] private UnityEngine.UI.Image CountryName;
    [SerializeField] private TMP_Text PlayerName;
    [SerializeField] private TMP_Text NumberOfCards;
    [SerializeField] private Animator animator;
    [Header("UI Containers")]
    [SerializeField] private Transform hpContainer;
    [SerializeField] private GameObject hpSlotPrefab;
    [SerializeField] private GameObject DarkLayer;

    [Header("HP Sprites")]
    [SerializeField] private Sprite fullHPSeedSprite;
    [SerializeField] private Sprite emptyHPSeedSprite;
    private List<UnityEngine.UI.Image> hpSeedImages = new List<UnityEngine.UI.Image>();
    public PlayerController assignPlayer { get; set; } = null;
    void Start()
    {
        InitializeUI();
    }
    void Update()
    {
        PlayerName.text = assignPlayer.name;
        NumberOfCards.text = assignPlayer.currentHand.Count.ToString();
        animator.SetBool("IsMyTurn", assignPlayer.isMyTurn());
        UpdateCurrentHP(assignPlayer.currentHP);
        UpdateBorderVisual();
    }
    void OnDestroy()
    {
        if (PlayerPortraitCreator.Instance != null)
        {
            PlayerPortraitCreator.Instance.SpawnedPortraits.Remove(this);
        }
    }
    public void InitializeUI()
    {
        SetupMaxHP(assignPlayer.maxHP);
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
        animator.SetBool("IsTargeted", isSelected);
    }
    public void SetupMaxHP(int maxHP)
    {
        foreach (Transform child in hpContainer)
        {
            Destroy(child.gameObject);
        }
        hpSeedImages.Clear();
        for (int i = 0; i < maxHP; i++)
        {
            GameObject newSeed = Instantiate(hpSlotPrefab, hpContainer);
            UnityEngine.UI.Image seedImage = newSeed.GetComponent<UnityEngine.UI.Image>();
            // Gán ảnh đầy máu mặc định lúc ban đầu
            seedImage.sprite = fullHPSeedSprite;
            // Lưu vào danh sách để quản lý bằng code sau này
            hpSeedImages.Add(seedImage);
        }
    }
    public void UpdateCurrentHP(int currentHP)
    {
        for (int i = 0; i < hpSeedImages.Count; i++)
        {
            // Nếu vị trí của hạt nằm trong tầm máu hiện tại -> Giữ nguyên hạt XANH
            if (i < currentHP)
            {
                hpSeedImages[i].sprite = fullHPSeedSprite;
            }
            // Nếu vượt quá lượng máu hiện tại -> Chuyển sang hạt RỖNG
            else
            {
                hpSeedImages[i].sprite = emptyHPSeedSprite;
            }
        }
    }
    public void UpdateTargetableState(bool isTargetable)
    {
        animator.SetBool("IsTargetable", isTargetable);
        DarkLayer.SetActive(isTargetable);
    }
}
