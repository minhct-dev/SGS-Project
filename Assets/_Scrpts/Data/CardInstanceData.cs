using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public struct CardInstanceData
{
    public string cardId;
    public int Number;
    public Suit Suit;
    public bool IsFaceUp;
    public CardInstanceData(CardInstance data)
    {
        cardId = data.CardID;
        Number = data.Number;
        Suit = data.Suit;
        IsFaceUp = data.IsFaceUp;
    }
    public CardData data
    {
        get
        {
            return CardData.Cache[cardId];
        }
    }
    //image , name , cosst , description, suit, number....
    public Sprite image => data.Image;
    public string name => data.CardName;
    public CardType cardType => data.CardType;
    public List<Effect> effects => data.Effects;
    public string description => data?.Description ?? "";
}

public class SyncListCardInstance : SyncList<CardInstanceData> { }

