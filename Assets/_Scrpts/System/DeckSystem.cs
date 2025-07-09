using System.Collections.Generic;
using UnityEngine;

public class DeckSystem : Singleton<DeckSystem>
{
    [SerializeField] private BasicCardData satCard;
    [SerializeField] private BasicCardData daoCard;

    [SerializeField] private ToolCardData toolDraw2Card;
    public List<CardInstance> BuildFullDeck()
    {
        var deck = new List<CardInstance>
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
        foreach (var card in deck)
        { 
            Debug.Log(card.Type);
        }

        return deck;
    }


}
