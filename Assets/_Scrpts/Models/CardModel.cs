using UnityEngine;

public abstract class CardModel
{
    public virtual string Name => Data.CardName;
    public virtual Sprite Image => Data.Image;
    public virtual CardType CardType => Data.CardType;
    public readonly CardData Data;

    public virtual bool IsFaceUp { get; set; } = true;          // lật ngửa/úp
    public virtual bool IsDisabled { get; set; } = false;       // vô hiệu (bị Vô Giải, v.v.)

    public CardModel(CardData data)
    {
        Data = data;
    }
}
