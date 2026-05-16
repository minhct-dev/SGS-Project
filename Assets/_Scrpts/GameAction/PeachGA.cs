using UnityEngine;

public class PeachGA : GameAction
{
    public PlayerController User { get; private set; }
    public PlayerController Target { get; private set; }
    public CardInstanceData PeachCard { get; private set; }
    public int HealAmount { get; private set; }

    public PeachGA(PlayerController User, PlayerController Target, CardInstanceData peachCard, int healAmount)
    {
        this.User = User;
        this.Target = Target;
        this.PeachCard = peachCard;
        this.HealAmount = healAmount;
    }

}