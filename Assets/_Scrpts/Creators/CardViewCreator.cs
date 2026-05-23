using UnityEngine;
using DG.Tweening;
public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView prefap;
    //create card view by prefap 
    public CardView CreateCardView(CardInstance Card, Vector3 position, Quaternion rotation, Transform parent)
    {
        CardView cardView = Instantiate(prefap, parent);
        RectTransform rect = cardView.GetComponent<RectTransform>(); //bug here
        rect.localPosition = Vector3.zero;
        rect.localRotation = rotation;
        rect.localScale = Vector3.zero;
        cardView.Setup(Card);
        rect.DOScale(0.7f, 0.15f).SetEase(Ease.OutBack);
        return cardView;
    }
}
