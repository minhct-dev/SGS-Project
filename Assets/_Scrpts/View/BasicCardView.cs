using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicCardView : CardView
{
    [Header("Poperty")]
    [field: SerializeField] public override GameObject wrapper { get; set; }
    [SerializeField] private TMP_Text CardName;
    [SerializeField] private TMP_Text CardNumber;
    [SerializeField] private TMP_Text CardSuit;
    //vừa mới chuyển từ sprite sang Image
    [SerializeField] private Image imageSR;

    public override void Setup(CardInstance card)
    {
        base.Setup(card);

        //BasicCardData basicData = card.Data as BasicCardData;
        CardName.text = card.Name;
        CardNumber.text = card.Number.ToString();
        CardSuit.text = card.Suit.ToSymbol();
        if (card.Image != null)
        {
            imageSR.sprite = card.Image;
            imageSR.preserveAspect = true;
        }
    }
}
