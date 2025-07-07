using System;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public abstract string CardName { get; set; }
    public abstract Sprite Image { get; set; }
    public abstract CardType CardType{ get; set; }

}
