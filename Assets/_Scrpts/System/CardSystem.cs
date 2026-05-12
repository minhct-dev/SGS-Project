using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using DG.Tweening;
using Mirror;
using Mirror.Examples.Basic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CardSystem : NetworkBehaviour
{
    [SerializeField] private HandView handView;
    [SerializeField] private PlayView playView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    [SerializeField] private Transform playCardViewPoint;
    //Biến thừa 
    private readonly List<CardInstance> drawPile = new();
    private readonly List<CardInstance> discardPile = new();

    public static CardSystem Instance;
    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        //ActionSystem.AttachPerformer<DiscardAllCardGA>(DiscardAllCardPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public override void OnStopServer()
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

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        playCardGA.user.currentHand.Remove(playCardGA.cardInstanceData);
        RpcPlayCardVisual(playCardGA.user, playCardGA.cardInstanceData);
        CardInstance card = playCardGA.cardInstanceData.ToCardInstance();
        foreach (var effect in card.Data.Effects)
        {
            GameAction generatedAction = effect.GetGameAction(
                playCardGA.user,
                playCardGA.targetIds,
                playCardGA.cardInstanceData
            );
            if (generatedAction != null)
            {
                ActionSystem.Instance.AddReaction(generatedAction);
            }
        }
        yield return null;
    }
    [ClientRpc]
    public void RpcPlayCardVisual(PlayerController user, CardInstanceData cardData)
    {
        VisualQueueSystem.Instance.EnqueueVisual(PlayCardVisualRountine(user, cardData));
    }
    //Corountine play card perform 
    private IEnumerator PlayCardVisualRountine(PlayerController user, CardInstanceData cardData)
    {
        bool isMe = (user == PlayerController.localPlayer);
        CardInstance card = cardData.ToCardInstance();
        if (isMe)
        {
            CardView cardView = handView.RemoveCard(card);
            if (cardView != null)
            {
                yield return playView.AddCard(cardView);
                yield return new WaitForSeconds(2f);
                yield return DiscardCard(cardView);
                PlayView.Instance.RemoveCard(cardView.Card);
            }
        }
        else
        {
            Debug.Log($"[{user.name}] vừa đánh lá [{card.Data.name}]");
        }

    }
    public IEnumerator DrawCard(CardInstance card)
    {
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation, handView.transform);
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
