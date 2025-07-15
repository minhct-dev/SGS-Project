using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ListExtensions
{
    //extension for drawing card
    public static T Draw<T>(this List<T> list)
    {
        if (list.Count == 0) return default;
        int r = Random.Range(0, list.Count);
        T t = list[r];
        list.Remove(t);
        return t;
    }
    //extension for exchanging from cardInstanceData from sever to cardinstance in client
    public static CardInstance ToCardInstance(this CardInstanceData data)
    {
        var rawData = CardData.Cache[data.cardId];
        if (data.cardType == CardType.BasicCard)
        {
            BasicCardData basicCardData = rawData as BasicCardData;
            return new CardInstance(basicCardData, data.Number, data.Suit)
            {
                IsFaceUp = data.IsFaceUp
            };

        }
        else if (data.cardType == CardType.ToolCard)
        {
            ToolCardData toolCardData = rawData as ToolCardData;
            return new CardInstance(toolCardData, data.Number, data.Suit)
            {
                IsFaceUp = data.IsFaceUp
            };
        }
        return null;
        
    }
}
