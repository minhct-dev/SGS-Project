using UnityEngine;

public static class CardSuitExtensions
{
    public static string ToSymbol(this Suit suit)
    {
        return suit switch
        {
            Suit.Heart => "♥",
            Suit.Diamond => "♦",
            Suit.Spade => "♠",
            Suit.Club => "♣",
            _ => "?"
        };
    }

    public static Color ToColor(this Suit suit)
    {
        return (suit == Suit.Heart || suit == Suit.Diamond) ? Color.red : Color.black;
    }
}
