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
    [SerializeField] private ToolCardData toolDraw2Card;
    [Header("UI Ref")]
    [SerializeField] private HandView handView;
    [Header("Game Item")]
    //drawPile is full of 160 playcard in SGS where decksystem will take card from here and send to player
    public List<CardInstance> drawPile { get; private set; } = new();

    //discardPile is where player discard card and card will add to discardPile
    public List<CardInstance> discardPile { get; private set; } = new();

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardPerform);
    }

    void OnDisable()
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
            new(daoCard, 6, Suit.Club)
        };

    }
    //refilldeck use to take all card in discardPile and refill the drawPile 
    [Server]
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }
    //lOGIC FUNCTION DrawCardLogicGA allow sever to draw card and put card data  into client data
    [Server]
    public void DrawCardLogicGA(DrawCardGA drawCardLogicGA)
    {
        if (drawCardLogicGA.Player == null) return;
        for (int i = 0; i < drawCardLogicGA.Amount; i++)
        {
            if (drawPile.Count == 0) RefillDeck();
            if (drawPile.Count == 0) break;

            CardInstance drawCard = drawPile.Draw();
            CardInstanceData drawCardData = new CardInstanceData(drawCard);
            drawCardLogicGA.Player.currentHand.Add(drawCardData);
            drawCardLogicGA.DrawCardList.Add(drawCardData);

        }
        TargetPerformDrawAction(drawCardLogicGA.Player.connectionToClient, drawCardLogicGA.DrawCardList.ToArray());

    }
    [TargetRpc]
    private void TargetPerformDrawAction(NetworkConnection conn, CardInstanceData[] cards)
    {
        PlayerController player = PlayerController.localPlayer;
        if (player != null)
        {
            DrawCardGA action = new DrawCardGA(player, cards.Length);
            action.DrawCardList.AddRange(cards);

            // Gọi ActionSystem thực hiện diễn hoạt
            ActionSystem.Instance.Perform(action);
        }
    }
    //DrawcardPerform will done at sever and while it add card into current hand of player
    //it will trigger a callback OnHandChanged
    public IEnumerator DrawCardPerform(DrawCardGA drawCardGA)
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
                yield return CardSystem.Instance.DrawCard(cardData.ToCardInstance());
            }
            else
            {
                //ui hiệu ứng rút bài của người khác nếu cần 
                yield return null;
            }
        }
    }



}
