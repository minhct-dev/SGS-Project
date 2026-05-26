using System.Security.Cryptography;
using DG.Tweening;
using Mono.Cecil;
using Telepathy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


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
    public static CardView CurrentlySelectedCard { get; private set; } = null;
    private static bool isOneCardSelected = false;
    public CardInstance Card { get; private set; }
    public Vector3 HandViewPosition { get; set; }



    public virtual void Setup(CardInstance card)
    {
        Card = card;
        CardNumber.text = card.Number.ToString();
        if (card.Suit == Suit.Heart || card.Suit == Suit.Diamond)
        {
            CardNumber.color = new Color(0.8f, 0.1f, 0.1f); // Đỏ thẫm
        }
        else
        {
            CardNumber.color = Color.black; // Đen
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
    public static void ForceUnselect()
    {
        if (CurrentlySelectedCard != null)
        {
            //card down animate
            CurrentlySelectedCard.transform.DOMoveY(CurrentlySelectedCard.transform.position.y - 1, 0.2f).SetEase(Ease.OutCubic);
            isOneCardSelected = false;
            CurrentlySelectedCard = null;
            if (InputTargetingSystem.Instance != null)
            {
                InputTargetingSystem.Instance.CancelSelection();
            }
        }

    }
    public static void ChooseCard()
    {
        isOneCardSelected = false;
        CurrentlySelectedCard = null;
    }
    public virtual GameObject GetWrapper()
    {
        return wrapper;
    }
    public virtual void OnMouseDown()
    {
        if (PlayView.Instance.playedCards.Contains(this)) return;
        if (isOneCardSelected && CurrentlySelectedCard == this)
        {
            ForceUnselect();
        }
        else
        {
            ForceUnselect();
            this.transform.DOMoveY(this.transform.position.y + 1, 0.2f).SetEase(Ease.OutCubic);
            CurrentlySelectedCard = this;
            isOneCardSelected = true;

            if (InputTargetingSystem.Instance != null)
            {
                InputTargetingSystem.Instance.OnCardClicked(this);
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
    public bool IsPlayable()
    {
        if (this.Card.Data.Effects.Count != 0)
        {
            return this.Card.Data.Effects[0].IsPlayable(PlayerController.localPlayer, TurnManagerSystem.Instance.currentPhase);
        }
        return true;
    }
}
