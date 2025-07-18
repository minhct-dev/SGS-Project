using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
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
    //PlayerController player, int amount
    [Server]
    public IEnumerator DrawCardPerform(DrawCardGA drawCardGA)
    {
        for (int i = 0; i < drawCardGA.Amount; i++)
        {
            CardInstance drawCard = drawPile.Draw();
            drawCardGA.Player.currentHand.Add(new CardInstanceData(drawCard));
        }
        return null;

    }


}
