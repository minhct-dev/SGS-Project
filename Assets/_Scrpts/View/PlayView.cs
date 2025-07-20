using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlayView : Singleton<PlayView>
{
    [SerializeField] private RectTransform PlayCardGroup;
    public readonly List<CardView> playedCards = new();

    public IEnumerator AddCard(CardView cardView)
    {
        //Debug.Log(cardView.Card.Number.ToString()+" "+cardView.Card.Suit.ToSymbol());
        playedCards.Add(cardView);
        yield return UpdateCardPosition(0.15f);
    }

    public CardView RemoveCard(CardInstance card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null) return null;
        playedCards.Remove(cardView);
        StartCoroutine(UpdateCardPosition(0.15f));
        return cardView;
    }
    public CardView GetCardView(CardInstance card)
    {
        return playedCards.Where(cardView =>
                            cardView.Card.CardId == card.CardId &&
                            cardView.Card.Number == card.Number &&
                            cardView.Card.Suit == card.Suit).FirstOrDefault();
    }
    private IEnumerator UpdateCardPosition(float duration)
    {
        if (playedCards.Count == 0) yield break;
        float totalWidth = PlayCardGroup.rect.width;
        float margin = 150f;
        float usableWidth = totalWidth - margin * 2f;
        float cardSpacing = Mathf.Min(300f, usableWidth / (playedCards.Count - 1));
        float firstCardPosition = PlayCardGroup.transform.position.x - (playedCards.Count - 1) * cardSpacing / 2;
        for (int i = 0; i < playedCards.Count; i++)
        {
            // Tính vị trí local bắt đầu từ trái
            float x = firstCardPosition + i * cardSpacing;
            Vector3 localPos = new Vector3(x, 0f, 0f);

            // Convert sang world position
            Vector3 worldPos = PlayCardGroup.TransformPoint(localPos);

            playedCards[i].transform.DOMove(worldPos, duration);

            playedCards[i].transform.DORotate(Vector3.zero, duration);
        }
        yield return new WaitForSeconds(duration);
    } 
    
    
}
