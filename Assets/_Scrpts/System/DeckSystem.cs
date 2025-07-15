using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class DeckSystem : NetworkBehaviour
{
    [SerializeField] private BasicCardData satCard;
    [SerializeField] private BasicCardData daoCard;
    [SerializeField] private ToolCardData toolDraw2Card;
    public List<CardInstance> drawPile { get; private set; } = new();
    public List<CardInstance> discardPile { get; private set; } = new();


    [Server]
    public void BuildFullDeck()
    {
        drawPile = new List<CardInstance>
        {
            new(satCard, 5, Suit.Spade),
            new(satCard, 8, Suit.Heart),
            new(toolDraw2Card, 13, Suit.Club),
            new(satCard, 6, Suit.Club),
            new(satCard, 6, Suit.Club),
            new(toolDraw2Card, 6, Suit.Club),
            new(toolDraw2Card, 5, Suit.Spade),
            new(toolDraw2Card, 8, Suit.Heart),
            new(daoCard, 13, Suit.Club),
            new(toolDraw2Card, 6, Suit.Club),
            new(daoCard, 6, Suit.Club),
            new(daoCard, 6, Suit.Club)
        };
        // foreach (var card in deck)
        // { 
        //     Debug.Log(card.Type);
        // }   //debuging :)
        RpcSyncDeck(drawPile.Select(card => new CardInstanceData(card)).ToList());
    }

    [ClientRpc]
    public void RpcSyncDeck(List<CardInstanceData> deckData)
    {
        if (deckData == null || deckData.Count == 0)
        {
            Debug.Log("recieved empty deck from sever!");
            return;
        }
        drawPile = deckData.Select(data => data.ToCardInstance()).ToList();
        Debug.Log("Client deck synced! " + drawPile.Count);
    }


}
