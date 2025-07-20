using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;

public class HandView : MonoBehaviour
{
    [SerializeField] private RectTransform HandViewGroup;
    private readonly List<CardView> cards = new();
    public bool oneCardSelected = false;
    public IEnumerator AddCard(CardView cardView)
    {
        cards.Add(cardView);
        yield return UpdateCardPosition(0.15f);
    }
    //Remove card in hand
    public CardView RemoveCard(CardInstance card)
    {
        ///Debug.Log(card.Number+" "+card.Suit.ToSymbol());
        CardView cardView = GetCardView(card);
        if (cardView == null) return null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPosition(0.15f));
        return cardView;
    }
    public CardView GetCardView(CardInstance card) {
        return cards.Where(cardView =>
                            cardView.Card.CardId == card.CardId &&
                            cardView.Card.Number == card.Number &&
                            cardView.Card.Suit == card.Suit).FirstOrDefault();
    }
    private IEnumerator UpdateCardPosition(float duration)
    {
        if (cards.Count == 0) yield break;

        float totalWidth = HandViewGroup.rect.width;
        float margin = 80f;
        float usableWidth = totalWidth - margin * 2f;
        float cardSpacing = Mathf.Min(140f, usableWidth / (cards.Count - 1));
        float firstCardPosition = HandViewGroup.transform.position.x - totalWidth / 2f + 80f;

        for (int i = 0; i < cards.Count; i++)
        {
            // Tính vị trí local bắt đầu từ trái
            float x = firstCardPosition + i * cardSpacing;
            Vector3 localPos = new Vector3(x, 0f, 0f);

            // Convert sang world position
            Vector3 worldPos = HandViewGroup.TransformPoint(localPos);
            cards[i].HandViewPosition = worldPos;

            cards[i].transform.DOMove(worldPos, duration);

            cards[i].transform.DORotate(Vector3.zero, duration);
        }
        yield return new WaitForSeconds(duration);
    } 
    
}
