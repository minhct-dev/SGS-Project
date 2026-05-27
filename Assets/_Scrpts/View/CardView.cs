using System.Security.Cryptography;
using DG.Tweening;
using Mono.Cecil;
using Telepathy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class CardView : MonoBehaviour
{
    [Header("UI References")]
    [field: SerializeField] public GameObject wrapper { get; set; }
    [SerializeField] private TMP_Text CardNumber;
    [SerializeField] private Image imageSR;

    [Header("Suit UI")]
    [SerializeField] private Image suitImage;
    [SerializeField] private Sprite spadeSprite;
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite clubSprite;
    [SerializeField] private Sprite diamondSprite;

    [Header("UI Property")]
    public static List<CardView> SelectedCards { get; private set; } = new();
    public static int MaxSelectionAmount = 1;
    //private static bool isOneCardSelected = false;
    public CardInstance Card { get; private set; }
    public Vector3 HandViewPosition { get; set; }

    public virtual void Setup(CardInstance card)
    {
        Card = card;
        CardNumber.text = card.Number.ToString();
        if (card.Suit == Suit.Heart || card.Suit == Suit.Diamond)
        {
            CardNumber.color = new Color(0.8f, 0.1f, 0.1f);
        }
        else
        {
            CardNumber.color = Color.black;
        }
        suitImage.sprite = GetSuitSprite(card.Suit);
        suitImage.type = Image.Type.Simple;
        suitImage.preserveAspect = true;
        if (card.Image != null)
        {
            imageSR.sprite = card.Image;
            imageSR.preserveAspect = true;
        }
    }

    private Sprite GetSuitSprite(Suit suit)
    {
        return suit switch
        {
            Suit.Spade => spadeSprite,
            Suit.Heart => heartSprite,
            Suit.Club => clubSprite,
            Suit.Diamond => diamondSprite,
            _ => null
        };
    }
    public static void ForceUnselectAll()
    {
        foreach (var card in SelectedCards)
        {
            if (card != null)
            {
                card.transform.DOMoveY(card.transform.position.y - 1, 0.2f).SetEase(Ease.OutCubic);
            }
        }
        SelectedCards.Clear();
        if (InputTargetingSystem.Instance != null)
        {
            InputTargetingSystem.Instance.CancelSelection();
        }
    }
    public static void ClearSelectionState()
    {
        SelectedCards.Clear();
    }
    public virtual void OnMouseDown()
    {
        if (PlayView.Instance.playedCards.Contains(this)) return;
        if (SelectedCards.Contains(this))
        {
            SelectedCards.Remove(this);
            this.transform.DOMoveY(this.transform.position.y - 1, 0.2f).SetEase(Ease.OutCubic);

            // Tùy chọn: Xử lý hủy ngắm mục tiêu nếu bỏ chọn bài
            if (SelectedCards.Count == 0 && InputTargetingSystem.Instance != null)
            {
                InputTargetingSystem.Instance.CancelSelection();
            }
        }
        else
        {
            switch (MaxSelectionAmount)
            {
                case 1:
                    ForceUnselectAll();
                    SelectedCards.Add(this);
                    this.transform.DOMoveY(this.transform.position.y + 1, 0.2f).SetEase(Ease.OutCubic);
                    if (InputTargetingSystem.Instance != null) InputTargetingSystem.Instance.OnCardClicked(this);
                    break;

                case > 1:
                    if (SelectedCards.Count < MaxSelectionAmount)
                    {
                        SelectedCards.Add(this);
                        this.transform.DOMoveY(this.transform.position.y + 1, 0.2f).SetEase(Ease.OutCubic);
                    }
                    else
                    {
                        Debug.Log($"[UI] Chỉ được chọn tối đa {MaxSelectionAmount} lá bài!");
                    }
                    break;

                default:
                    Debug.Log("Max selection amount <= 0");
                    break;
            }
        }
    }
    public virtual void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log(this.Card.Name);
            ToolTip.Instance.ShowToolTip(this.Card.Name, this.Card.Description);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ToolTip.Instance.HideToolTip();
        }

    }
    public virtual void OnMouseExit()
    {
        ToolTip.Instance.HideToolTip();
    }
    //chuyển qua multiple choice sẽ có sự khác biệt khi dùng hàm này
    public bool IsPlayable()
    {
        if (this.Card.Data.Effects.Count != 0)
        {
            return this.Card.Data.Effects[0].IsPlayable(PlayerController.localPlayer, TurnManagerSystem.Instance.currentPhase);
        }
        return true;
    }
}
