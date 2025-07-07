using UnityEngine;

public abstract class CardModel
{
    public string Name => Data.name;
    public Sprite Image => Data.Image;
    public readonly CardData Data;

    public bool IsFaceUp { get; set; } = true;          // lật ngửa/úp
    public bool IsDisabled { get; set; } = false;       // vô hiệu (bị Vô Giải, v.v.)

    public CardModel(CardData data)
    {
        Data = data;
    }
}
