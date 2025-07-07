using UnityEngine;
using DG.Tweening;
public class CardViewHoveSystem : Singleton<CardViewHoveSystem>
{
    [SerializeField] private CardView cardViewHover;
    private CardView currentSelectedCard;
    public void Show(CardInstance card, Vector3 oldPosition, Vector3 position, CardView source)
    {
        if (currentSelectedCard != null && currentSelectedCard != source)
        {
            currentSelectedCard.ForceUnselect(); // gọi hàm bên CardView
        }

        currentSelectedCard = source;

        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(card);
        cardViewHover.transform.position = oldPosition;
        cardViewHover.transform.DOMove(position, 0.2f).SetEase(Ease.OutCubic);
    }
    public void Hide(Vector3 pos, CardView source)
    {
        if (currentSelectedCard == source)
        { 
            currentSelectedCard = null;
            cardViewHover.transform.DOMove(pos, 0.2f).SetEase(Ease.OutCubic);
            cardViewHover.gameObject.SetActive(false); 
        }
        
    }
}
