using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ToolCardView : CardView
{
    [Header("Poperty")]
    [SerializeField] private TMP_Text CardName;
    [SerializeField] private TMP_Text CardNumber;
    [SerializeField] private TMP_Text CardSuit;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private TMP_Text Description;
    [field: SerializeField] public override GameObject wrapper { get; set; }

    public override void Setup(CardInstance card)
    {
        base.Setup(card);
        CardName.text = card.Name;
        CardNumber.text = card.Number.ToString();
        CardSuit.text = card.Suit.ToSymbol();
        imageSR.sprite = card.Image;
        Description.text = card.GetDescription();
    }
}
