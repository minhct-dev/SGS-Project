using UnityEngine;

public class BreakGA : GameAction
{
    public PlayerController User { get; private set; }
    public PlayerController Target { get; private set; }
    public CardInstanceData SourceCard { get; private set; }

    public BreakGA(PlayerController user, PlayerController target, int amount, CardInstanceData sourceCard)
    {
        this.User = user;
        this.Target = target;
        this.SourceCard = sourceCard;
    }

}