using System;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private Sprite image;
    [SerializeField] private CardType cardType;
    [SerializeField] private List<Effect> effects;

    public string CardName => cardName;
    public Sprite Image => image;
    public CardType CardType => cardType;
    public List<Effect> Effects => effects;
}
