using System;
using System.Collections.Generic;
using System.Linq;
using SerializeReferenceEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData")]
public class CardData : ScriptableObject
{
    [SerializeField] string id = "";
    [SerializeField] private string cardName;
    [SerializeField] private Sprite image;
    [SerializeField] private CardType cardType;
    [SerializeField] private int requiredTarget;
    [SerializeField] private string description;
    [field: SerializeReference, SR] private List<Effect> effects;


    public string CardId => id;
    public string CardName => cardName;
    public Sprite Image => image;
    public CardType CardType => cardType;
    public string Description => description;
    public List<Effect> Effects => effects;
    public int RequiredTarget => requiredTarget;
    static Dictionary<string, CardData> _cache;

    public static Dictionary<string, CardData> Cache
    {
        get
        {
            if (_cache == null)
            {
                // Load all ScriptableCards from our Resources folder
                CardData[] cards = Resources.LoadAll<CardData>("Data");

                _cache = cards.ToDictionary(card => card.id, card => card);
            }
            return _cache;
        }
    }
    private void OnValidate()
    {
        // Get a unique identifier from the asset's unique 'Asset Path' (ex : Resources/Weapons/Sword.asset)
        // You're free to set your own uniqueIDs instead of using this current system, but unless
        // you know what you're doing, I wouldn't recommend changing this in the inspector.
        // If you do change it and want to change back, just erase the uniqueID in the inspector and it will refill itself.
        if (id == "")
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(path);
#endif
        }
    }
}
