using TMPro;
using UnityEngine;

public class BasicCardView : CardView
{
    [Header("Poperty")]
    [field: SerializeField] public override GameObject wrapper { get; set; }
    [SerializeField] private TMP_Text CardName;
    [SerializeField] private TMP_Text CardNumber;
    [SerializeField] private TMP_Text CardSuit;
    [SerializeField] private SpriteRenderer imageSR;

    public override void Setup(CardInstance card)
    {
        base.Setup(card);
        BasicCardData basicData = card.Data as BasicCardData;
        CardName.text = basicData.CardName;
        CardNumber.text = card.Number.ToString();
        CardSuit.text = card.Suit.ToSymbol();
        imageSR.sprite = basicData.Image;
    }
}
