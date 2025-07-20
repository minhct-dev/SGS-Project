using UnityEngine;

public class PlayCardGA : GameAction
{
    public CardInstanceData cardInstanceData;
    public PlayerController user;
    public PlayCardGA(PlayerController user, CardInstanceData cardInstanceData)
    {
        this.cardInstanceData = cardInstanceData;
        this.user = user;
    }
}
