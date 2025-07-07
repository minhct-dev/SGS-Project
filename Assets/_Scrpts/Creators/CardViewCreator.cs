using UnityEngine;
using DG.Tweening;
public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefap;
    
    public CardView CreateCardView(CardInstance Card, Vector3 position, Quaternion rotation)
    {
        CardView cardView = Instantiate(cardViewPrefap, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, 0.15f);
        cardView.Setup(Card);
        return cardView;
    }
}
