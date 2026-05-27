using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
    }

    public override void OnStopServer()
    {
        //ActionSystem.DetachPerformer<DiscardAllCardGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    //publics
    public void Setup()
    {
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
        StartCoroutine(PlayCardVisualRountine(user, cardData));
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
                //----------
            }
        }
        else
        {
            Debug.Log($"[{user.name}] vừa đánh lá [{card.Data.name}]");
            UnityEngine.Vector3 spawnPosition = user.playerPosition;
            CardView opponentCardView = CardViewCreator.Instance.CreateCardView(
                card,
                spawnPosition,
                UnityEngine.Quaternion.identity,
                playView.transform // Đặt PlayView làm cha để render chuẩn UI
            );
            if (opponentCardView != null)
            {
                opponentCardView.transform.position = spawnPosition;
                opponentCardView.transform.localScale = UnityEngine.Vector3.zero;
                opponentCardView.transform.DOScale(0.7f, 0.4f);
                yield return opponentCardView.transform.DOMove(playView.transform.position, 0.4f).SetEase(Ease.OutQuad);
                yield return playView.AddCard(opponentCardView);
                //-----

            }

        }

    }
    [ClientRpc]
    public void RpcClearPlayView()
    {
        VisualQueueSystem.Instance.EnqueueVisual(ClearPlayViewRountine());
    }
    private IEnumerator ClearPlayViewRountine()
    {
        yield return new WaitForSeconds(1.5f);
        List<CardView> cardsOnBoard = new List<CardView>(PlayView.Instance.playedCards);
        foreach (var cardView in cardsOnBoard)
        {
            PlayView.Instance.RemoveCard(cardView.Card);
            StartCoroutine(DiscardCard(cardView));
        }
        if (PlayerController.localPlayer != null)
        {
            PlayerController.localPlayer.isWaitingForServer = false;
        }
        yield return null;
    }
    public IEnumerator DrawCard(CardInstance card)
    {
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation, handView.transform);
        yield return handView.AddCard(cardView);
    }
    private IEnumerator DiscardCard(CardView cardView)
    {
        discardPile.Add(cardView.Card);
        cardView.transform.DOScale(UnityEngine.Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }


}
