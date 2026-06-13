using System;
using System.Runtime.CompilerServices;
using IO.Swagger.Model;
using Mirror.Examples.Basic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class TargetInventoryUI : Singleton<TargetInventoryUI>
{
    [SerializeField] private GameObject TargetInventoryPanel;
    [SerializeField] private TMP_Text PanelName;

    [Header("Container (Horizontal layout group)")]
    [SerializeField] private Transform handContainer;
    //[SerializeField] private Transform equipContainer;
    //[SerializeField] private Transform JudgmentContainer;

    [Header("Prefabs")]
    [SerializeField] private GameObject cardBackPrefab;
    //[SerializeField] private GameObject cardFacePrefab;
    private PlayerController currentTarget;
    //add cardInstanceData[] equip, judgments 
    protected override void Awake()
    {
        base.Awake();
        ForceClosePanel();
    }
    public void OpenPanel(PlayerController target)
    {
        currentTarget = target;
        ClearItems();
        // if (PanelName != null)
        // {
        //     PanelName.text = breakGA.SourceCard.name;
        // }
        //Spawn hand card 
        for (int i = 0; i < currentTarget.currentHand.Count; i++)
        {
            GameObject cardObj = Instantiate(cardBackPrefab, handContainer);
            int index = 1;
            Button btn = cardObj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnCardSelected(CardArea.Hand, index, ""));
        }
    }
    private void OnCardSelected(CardArea area, int handIndex, string cardId)
    {
        if (currentTarget == null) return;

        // 1. Bắn lệnh lên Server ngay lập tức với dữ liệu vừa bấm
        //PlayerController.localPlayer.CmdInteractWithTargetCard(area, handIndex, cardId);

        // 2. Đóng và dọn dẹp UI ngay tức thì
        ForceClosePanel();
    }
    private void ClearItems()
    {
        foreach (Transform child in handContainer) Destroy(child.gameObject);
    }
    public void ForceClosePanel()
    {
        TargetInventoryPanel.SetActive(false);
        ClearItems();
        currentTarget = null;
    }
}
