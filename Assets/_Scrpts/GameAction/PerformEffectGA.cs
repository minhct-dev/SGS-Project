using UnityEngine;

public class PerformEffectGA : GameAction
{
    public Effect Effect { get; private set; }
    public PlayerController user{ get; private set; }
    public PerformEffectGA(Effect effect, PlayerController user)
    {
        this.Effect = effect;
        this.user = user;
    }
}
