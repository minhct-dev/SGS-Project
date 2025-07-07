using UnityEngine;

public class PlayCardGA : GameAction
{
    public CardInstance cardInstance;
    public PlayCardGA(CardInstance cardInstance)
    {
        this.cardInstance = cardInstance;
    }
}
