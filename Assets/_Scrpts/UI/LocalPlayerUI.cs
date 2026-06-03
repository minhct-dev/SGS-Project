using Microsoft.Unity.VisualStudio.Editor;
using Mirror;
using Mirror.Examples.Basic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LocalPlayerUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image CommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image DeputyCommanderPortrait;
    [SerializeField] private UnityEngine.UI.Image AvatarPanel;
    [SerializeField] private UnityEngine.UI.Image CountryName;
    [SerializeField] private TMP_Text PlayerName;
    [Header("UI Containers")]
    [SerializeField] private Transform hpContainer;
    [SerializeField] private GameObject hpSlotPrefab;

    [Header("HP Sprites")]
    [SerializeField] private Sprite fullHPSeedSprite;
    [SerializeField] private Sprite emptyHPSeedSprite;
    private PlayerController localPlayer;
    //[field: SerializeField] private GameObject wrapper { get; set; }
    private List<UnityEngine.UI.Image> hpSeedImages = new List<UnityEngine.UI.Image>();

    void Update()
    {
        if (localPlayer == null) return;
        //wrapper.SetActive(true);
        //Debug.Log("connect to local success , Name:" + player.name + " , maxHP =" + player.maxHP +", currHP ="+player.currentHP);
        PlayerName.text = localPlayer.username;
        UpdateCurrentHP(localPlayer.currentHP);
        //if (player && player.hasEnemy) enemyInfo = player.enemyInfo;
    }
    public void InitializeUI(PlayerController player)
    {
        localPlayer = player;
        SetupMaxHP(player.maxHP);
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
}
