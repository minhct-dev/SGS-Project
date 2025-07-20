using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using DG.Tweening;
using Mirror.Examples.Basic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private PlayView playView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    [SerializeField] private Transform playCardViewPoint;
    private readonly List<CardInstance> drawPile = new();
    private readonly List<CardInstance> discardPile = new();
    
    void OnEnable()
    {
        //ActionSystem.AttachPerformer<DiscardAllCardGA>(DiscardAllCardPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    void OnDisable()
    {
        //ActionSystem.DetachPerformer<DiscardAllCardGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    //publics
    public void Setup()
    {  
    }
    //Reactions
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        CardView currectSelectedCard = CardView.CurrentlySelectedCard;
        if (currectSelectedCard != null)
        {
            ///CardViewHoveSystem.Instance.Hide(currectSelectedCard);
        }
        DiscardAllCardGA discardAllCardGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardGA);
    }
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        //DrawCardGA drawCardGA = new(5);
        //ActionSystem.Instance.AddReaction(drawCardGA);
    }

    //Performers
    // private IEnumerator DiscardAllCardPerformer(DiscardAllCardGA discardAllCardGA)
    // {
    //     foreach (CardInstance card in hand)
    //     {
    //         discardPile.Add(card);
    //         CardView cardView = handView.RemoveCard(card);
    //         yield return DiscardCard(cardView);
    //     }
    //     hand.Clear();
    // }
    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        PlayerController user = playCardGA.user;
        if (user.playerType == PlayerType.LOCAL)
        {
            //remove lá bài trong list
            CardView cardView = handView.RemoveCard(playCardGA.cardInstanceData.ToCardInstance());
            //Debug.Log(cardView);
            //CardViewHoveSystem.Instance.Hide(cardView);
            yield return playView.AddCard(cardView);
            yield return new WaitForSeconds(2f);
            yield return DiscardCard(cardView);
            PlayView.Instance.RemoveCard(cardView.Card);
        }
        
        //perform effect
        foreach (var effect in playCardGA.cardInstanceData.ToCardInstance().Data.Effects)
        {
            PerformEffectGA performEffectGA = new(effect,playCardGA.user);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }
    
    //Helpers
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }

    public IEnumerator DrawCard(CardInstance card)
    {
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
