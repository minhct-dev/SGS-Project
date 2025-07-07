using UnityEngine;

public class BasicCardModel : CardModel
{
    public readonly BasicCardData basicCardData;
    public BasicCardModel(BasicCardData data) : base(data)
    {
        basicCardData = data;
    }
}
