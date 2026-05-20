using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Mirror;
using Mirror.Examples.Basic;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;
public class DeckEntry
{
    public string CardName; // Phải trùng tên file ScriptableObject
    public int Number;
    public string Suit;
}
public class DeckData
{
    public List<DeckEntry> Deck;
}
public class DeckSystem : NetworkBehaviour
{
    [Header("UI Ref")]
    [SerializeField] private HandView handView;
    [Header("Game Item")]
    //drawPile is full of 160 playcard in SGS where decksystem will take card from here and send to player
    [SerializeField] private CardSystem cardSystem;
    [SerializeField] public List<CardInstance> drawPile;

    //discardPile is where player discard card and card will add to discardPile
    [SerializeField] public List<CardInstance> discardPile;
    [Header("Data File")]
    [SerializeField] private TextAsset deckJsonFile;
    private Dictionary<string, CardData> soCache = new Dictionary<string, CardData>();
    public override void OnStartServer()
    {
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardPerform);
    }

    public override void OnStopServer()
    {
        ActionSystem.DetachPerformer<DrawCardGA>();
    }
    private CardData LoadCardScripableObject(string cardName)
    {
        if (soCache.ContainsKey(cardName))
            return soCache[cardName];

        // Đường dẫn tương đối tính từ thư mục Resources
        CardData loadedSO = Resources.Load<CardData>($"Data/Card/{cardName}");

        if (loadedSO != null)
        {
            soCache.Add(cardName, loadedSO);
            return loadedSO;
        }
        else
        {
            Debug.LogError($"Không tìm thấy ScriptableObject nào tên là '{cardName}' trong thư mục Resources/Card!");
            return null;
        }
    }
    [Server]
    //Use to create full deck from the begining of the match 
    public void BuildFullDeck()
    {
        drawPile.Clear();
        if (deckJsonFile == null)
        {
            Debug.LogError("Chưa gắn file StandardDeck.json vào DeckSystem!");
            return;
        }

        // 3. Đọc dữ liệu từ file JSON
        DeckData deckData = JsonConvert.DeserializeObject<DeckData>(deckJsonFile.text);

        // 4. Vòng lặp đúc bài
        foreach (DeckEntry entry in deckData.Deck)
        {
            // Tải ScriptableObject từ thư mục Resources/Cards
            CardData cardSO = LoadCardScripableObject(entry.CardName);

            if (cardSO != null)
            {
                // Ép kiểu chuỗi sang Enum Suit
                if (Enum.TryParse(entry.Suit, out Suit parsedSuit))
                {
                    // Đúc lá bài mới và nhét vào xấp bài
                    drawPile.Add(new CardInstance(cardSO, entry.Number, parsedSuit));
                }
                else
                {
                    Debug.LogError($"Sai tên chất bài ({entry.Suit}) ở lá {entry.CardName}");
                }
            }
        }

        // Đảo bài (Giả sử bạn có hàm Shuffle cho List)
        // drawPile.Shuffle(); 
        Debug.Log($"Đã tạo xong xấp bài với {drawPile.Count} lá!");

    }


    //refilldeck use to take all card in discardPile and refill the drawPile 
    [Server]
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }
    private IEnumerator DrawCardPerform(DrawCardGA drawCardGA)
    {
        if (drawCardGA.Player == null) yield break;
        for (int i = 0; i < drawCardGA.Amount; i++)
        {
            if (drawPile.Count == 0) RefillDeck();
            if (drawPile.Count == 0) break;
            CardInstance drawCard = drawPile.Draw();
            CardInstanceData drawCardData = new CardInstanceData(drawCard);
            drawCardGA.Player.currentHand.Add(drawCardData);
            drawCardGA.DrawCardList.Add(drawCardData);
        }
        TargetPerformDrawVisual(drawCardGA.Player.connectionToClient, drawCardGA);
        CardSystem.Instance.RpcClearPlayView();
        yield return null;
    }
    [TargetRpc]
    private void TargetPerformDrawVisual(NetworkConnection conn, DrawCardGA drawCardGA)
    {
        VisualQueueSystem.Instance.EnqueueVisual(DrawVisualRountine(drawCardGA));
    }
    public IEnumerator DrawVisualRountine(DrawCardGA drawCardGA)
    {
        Debug.Log("Máy " + drawCardGA.Player.name + "perform rút " + drawCardGA.Amount + " lá bài");
        if (drawCardGA.Player == null || drawCardGA.DrawCardList.Count == 0) yield break;
        //Debug.Log($"Visual: {drawCardGA.Player.name} đang hiển thị {drawCardGA.DrawCardList.Count} lá bài.");
        bool isMe = (drawCardGA.Player == PlayerController.localPlayer);
        foreach (var cardData in drawCardGA.DrawCardList)
        {
            //Debug.Log("Check local: " + isMe);
            if (isMe)
            {
                yield return cardSystem.DrawCard(cardData.ToCardInstance());
            }
            else
            {
                //ui hiệu ứng rút bài của người khác nếu cần 
                yield return null;
            }
        }
    }





}
