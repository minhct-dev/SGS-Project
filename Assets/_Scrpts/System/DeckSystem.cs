using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.Examples.Basic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckSystem : NetworkBehaviour
{
    [Header("Card Data")]
    [SerializeField] private BasicCardData satCard;
    [SerializeField] private BasicCardData daoCard;
    [SerializeField] private BasicCardData thiemCard;
    [SerializeField] private ToolCardData toolDraw2Card;
    [Header("UI Ref")]
    [SerializeField] private HandView handView;
    [Header("Game Item")]
    //drawPile is full of 160 playcard in SGS where decksystem will take card from here and send to player
    [SerializeField] private CardSystem cardSystem;
    [SerializeField] public List<CardInstance> drawPile;

    //discardPile is where player discard card and card will add to discardPile
    [SerializeField] public List<CardInstance> discardPile;

    public override void OnStartServer()
    {
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardPerform);
    }

    public override void OnStopServer()
    {
        ActionSystem.DetachPerformer<DrawCardGA>();
    }

    [Server]
    //Use to create full deck from the begining of the match 
    public void BuildFullDeck()
    {
        drawPile = new List<CardInstance>
        {
            new(satCard, 5, Suit.Spade),
            new(satCard, 8, Suit.Heart),
            new(toolDraw2Card, 13, Suit.Club),
            new(satCard, 6, Suit.Club),
            new(satCard, 6, Suit.Club),
            new(toolDraw2Card, 6, Suit.Club),
            new(toolDraw2Card, 5, Suit.Spade),
            new(toolDraw2Card, 8, Suit.Heart),
            new(daoCard, 13, Suit.Club),
            new(toolDraw2Card, 6, Suit.Club),
            new(daoCard, 6, Suit.Club),
            new(daoCard, 6, Suit.Club),
            new(toolDraw2Card, 8, Suit.Spade),
            new(toolDraw2Card, 9, Suit.Spade),
            // new(toolDraw2Card, 9, Suit.Spade),
            // new(toolDraw2Card, 10, Suit.Spade),
            // new(toolDraw2Card, 10, Suit.Spade),
            new(thiemCard, 2, Suit.Diamond),
            new(thiemCard, 2, Suit.Heart),
            new(thiemCard, 11, Suit.Heart),
            new(thiemCard, 13, Suit.Diamond),
        };

    }
    //refilldeck use to take all card in discardPile and refill the drawPile 
    [Server]
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }
    private IEnumerator DrawCardPerform(DrawCardGA drawCardGA)
    {
        if (drawCardGA.Player == null) yield break;
        for (int i = 0; i < drawCardGA.Amount; i++)
        {
            if (drawPile.Count == 0) RefillDeck();
            if (drawPile.Count == 0) break;
            CardInstance drawCard = drawPile.Draw();
            CardInstanceData drawCardData = new CardInstanceData(drawCard);
            drawCardGA.Player.currentHand.Add(drawCardData);
            drawCardGA.DrawCardList.Add(drawCardData);
        }
        TargetPerformDrawVisual(drawCardGA.Player.connectionToClient, drawCardGA);
        yield return null;
    }
    [TargetRpc]
    private void TargetPerformDrawVisual(NetworkConnection conn, DrawCardGA drawCardGA)
    {
        VisualQueueSystem.Instance.EnqueueVisual(DrawVisualRountine(drawCardGA));
    }
    public IEnumerator DrawVisualRountine(DrawCardGA drawCardGA)
    {
        Debug.Log("Máy " + drawCardGA.Player.name + "perform rút " + drawCardGA.Amount + " lá bài");
        if (drawCardGA.Player == null || drawCardGA.DrawCardList.Count == 0) yield break;
        //Debug.Log($"Visual: {drawCardGA.Player.name} đang hiển thị {drawCardGA.DrawCardList.Count} lá bài.");
        bool isMe = (drawCardGA.Player == PlayerController.localPlayer);
        foreach (var cardData in drawCardGA.DrawCardList)
        {
            //Debug.Log("Check local: " + isMe);
            if (isMe)
            {
                yield return cardSystem.DrawCard(cardData.ToCardInstance());
            }
            else
            {
                //ui hiệu ứng rút bài của người khác nếu cần 
                yield return null;
            }
        }
    }





}
