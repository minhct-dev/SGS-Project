using UnityEngine;

[System.Serializable]
public class CardInstance
{
    public readonly CardData Data;
    public string Name => Data.CardName;
    public string Description => Data.Description;
    public CardType Type => Data.CardType;
    public string CardID => Data.CardId;
    public Sprite Image => Data.Image;    // Trỏ đến dữ liệu gốc
    public int Number { get; private set; }         // 1–13
    public Suit Suit { get; private set; }          // ♠ ♥ ♦ ♣
    public bool IsFaceUp { get; set; } = true;
    public bool IsDisabled { get; set; } = false;


    public CardInstance(CardData data, int number, Suit suit)
    {
        Data = data;
        Number = number;
        Suit = suit;
    }
}
