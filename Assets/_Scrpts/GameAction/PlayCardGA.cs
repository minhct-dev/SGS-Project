using UnityEngine;
//PlayCardGA is a game action represent for action play 1 card of player 
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
