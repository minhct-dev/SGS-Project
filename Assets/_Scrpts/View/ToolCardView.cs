using TMPro;
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
        ToolCardData toolData = card.Data as ToolCardData;
        CardName.text = toolData.CardName;
        CardNumber.text = card.Number.ToString();
        CardSuit.text = card.Suit.ToSymbol();
        imageSR.sprite = toolData.Image;
        Description.text = toolData.Description;
    }
}
