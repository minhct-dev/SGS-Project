using UnityEngine;
using DG.Tweening;
public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView basicCardViewPrefap;
    [SerializeField] private CardView toolCardViewPrefap;
    //create card view by prefap 
    public CardView CreateCardView(CardInstance Card, Vector3 position, Quaternion rotation)
    {
        CardView cardView = Instantiate(GetPrefabForCard(Card), position, rotation); //bug here
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, 0.15f);
        cardView.Setup(Card);
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
