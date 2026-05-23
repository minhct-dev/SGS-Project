using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ListExtensions
{
    //extension for drawing card
    public static T Draw<T>(this List<T> list)
    {
        if (list.Count == 0) return default;
        int r = Random.Range(0, list.Count);
        T t = list[r];
        list.Remove(t);
        return t;
    }
    //extension for exchanging from cardInstanceData from sever to cardinstance in client
    public static CardInstance ToCardInstance(this CardInstanceData data)
    {
        if (CardData.Cache.TryGetValue(data.cardId, out CardData rawData))
        {
            return new CardInstance(rawData, data.Number, data.Suit)
            {
                IsFaceUp = data.IsFaceUp
            };
        }
        // Báo lỗi đỏ lên Console nếu Server gửi về một ID ma mà Client không có
        Debug.LogError($"Không tìm thấy thẻ bài nào với ID: {data.cardId} trong Cache!");
        return null;
    }
}
