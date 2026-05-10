using UnityEngine;
using DG.Tweening;
public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView basicCardViewPrefap;
    [SerializeField] private CardView toolCardViewPrefap;
    //create card view by prefap 
    public CardView CreateCardView(CardInstance Card, Vector3 position, Quaternion rotation, Transform parent)
    {
        CardView cardView = Instantiate(GetPrefabForCard(Card), parent);
        RectTransform rect = cardView.GetComponent<RectTransform>(); //bug here
        rect.localPosition = Vector3.zero;
        rect.localRotation = rotation;
        rect.localScale = Vector3.zero;
        cardView.Setup(Card);
        rect.DOScale(0.3f, 0.15f).SetEase(Ease.OutBack);
        return cardView;
    }

    private CardView GetPrefabForCard(CardInstance card)
    {
        return card.Type switch
        {
            CardType.BasicCard => basicCardViewPrefap,
            CardType.ToolCard => toolCardViewPrefap,
            _ => throw new System.Exception("Unknown card type: " + card.Type)
        };
    }
}
