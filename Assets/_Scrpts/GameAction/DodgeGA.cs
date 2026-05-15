using UnityEngine;

public class DodgeGA : GameAction
{
    public PlayerController User;
    public CardInstanceData DodgeCard;
    public SlashGA TargetDamageAction;
    public DodgeGA(PlayerController user, CardInstanceData dodgeCard, SlashGA targetDamageAction)
    {
        this.User = user;
        this.TargetDamageAction = targetDamageAction;
        this.DodgeCard = dodgeCard;
    }

}