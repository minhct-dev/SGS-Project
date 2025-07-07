using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private PlayView playView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    [SerializeField] private Transform playCardViewPoint;
    private readonly List<CardInstance> drawPile = new();
    private readonly List<CardInstance> discardPile = new();
    private readonly List<CardInstance> hand = new();
    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardGA>(DiscardAllCardPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardGA>();
        ActionSystem.DetachPerformer<DiscardAllCardGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    //publics
    public void Setup()
    {
        foreach (CardInstance card in DeckSystem.Instance.BuildFullDeck())
        { 
            drawPile.Add(card);
        }    
    }
    //Reactions
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        CardView currectSelectedCard = CardViewHoveSystem.Instance.currentSelectedCard;
        if (currectSelectedCard != null)
        {
            CardViewHoveSystem.Instance.Hide(currectSelectedCard);
        }
        DiscardAllCardGA discardAllCardGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardGA);
    }
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        DrawCardGA drawCardGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardGA);
    }

    //Performers
    private IEnumerator DrawCardsPerformer(DrawCardGA drawCardGA)
    {
        int actualAmount = Mathf.Min(drawCardGA.Amount, drawPile.Count);
        int notDrawAmount = drawCardGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if (notDrawAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawAmount; i++)
            {
                yield return DrawCard();
            }
        }
        yield return null;
    }

    private IEnumerator DiscardAllCardPerformer(DiscardAllCardGA discardAllCardGA)
    {
        foreach (CardInstance card in hand)
        {
            discardPile.Add(card);
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }
    //perform đang có vấn đề 
    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.cardInstance); //remove lá bài trong list
        CardView cardView = handView.RemoveCard(playCardGA.cardInstance);
        CardViewHoveSystem.Instance.Hide(cardView);
        playView.AddCard(cardView);
        //Tween tween = cardView.transform.DOMove(playCardViewPoint.position,0.15f);
        //yield return tween.WaitForCompletion();
        yield return new WaitForSeconds(2f);
        //yield return DiscardCard(cardView);
        //perform effect
    }
    
    //Helpers
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();

    }

    private IEnumerator DrawCard()
    {
        CardInstance card = drawPile.Draw();
        hand.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }
    private IEnumerator DiscardCard(CardView cardView)
    {
        discardPile.Add(cardView.Card);
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }


}
