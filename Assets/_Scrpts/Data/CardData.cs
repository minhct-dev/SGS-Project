using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    [SerializeField] protected string cardName;
    [SerializeField] protected Sprite image;
    [SerializeField] protected CardType cardType;
    [SerializeField] protected List<Effect> effects;

    public virtual string CardName { get; set ; }
    public virtual Sprite Image { get ; set ; }
    public virtual CardType CardType { get ; set ; }
    public virtual List<Effect> Effects { get ; set ; }

}
