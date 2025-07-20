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
    //refilldeck use to take all card in discardPile and refill the drawPile 
    [Server]
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }
    //DrawcardPerform will done at sever and while it add card into current hand of player
    //it will trigger a callback OnHandChanged
    [Server]
    public IEnumerator DrawCardPerform(DrawCardGA drawCardGA)
    {
        int actualAmount = Mathf.Min(drawCardGA.Amount, drawPile.Count);
        int notDrawAmount = drawCardGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            CardInstance drawCard = drawPile.Draw();
            drawCardGA.Player.currentHand.Add(new CardInstanceData(drawCard));
        }
        if (notDrawAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawAmount; i++)
        {
            CardInstance drawCard = drawPile.Draw();
            drawCardGA.Player.currentHand.Add(new CardInstanceData(drawCard));
        }
        }
        //mightbug here dueto network speed
        yield return drawCardGA.Player.ProcessDrawCards();
    }


}
