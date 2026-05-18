using UnityEngine;

public class DodgeGA : GameAction
{
    public PlayerController User;
    public CardInstanceData DodgeCard;
    public DodgeGA(PlayerController user, CardInstanceData dodgeCard)
    {
        this.User = user;
        this.DodgeCard = dodgeCard;
    }
}