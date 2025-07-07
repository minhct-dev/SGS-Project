using System.Collections.Generic;
using UnityEngine;

public class DeckSystem : Singleton<DeckSystem>
{
    [SerializeField] private BasicCardData satCard;
    public List<CardInstance> BuildFullDeck()
    {
        var deck = new List<CardInstance>
        {
            new(satCard, 5, Suit.Spade),
            new(satCard, 8, Suit.Heart),
            new(satCard, 13, Suit.Club),
            new(satCard, 6, Suit.Club),
            new(satCard, 6, Suit.Club),
            new(satCard, 6, Suit.Club)
        };
        // Shuffle
        return deck;
    }


}
