using UnityEngine;

public class DodgeGA : GameAction
{
    public PlayerController User;
    public CardInstanceData DodgeCard;
    public DealDamageGA TargetDamageAction;
    public DodgeGA(PlayerController user, CardInstanceData dodgeCard, DealDamageGA targetDamageAction)
    {
        this.User = user;
        this.TargetDamageAction = targetDamageAction;
        this.DodgeCard = dodgeCard;
    }

}