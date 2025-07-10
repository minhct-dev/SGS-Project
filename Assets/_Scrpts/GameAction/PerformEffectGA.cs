using UnityEngine;

public class PerformEffectGA : GameAction
{
    public Effect Effect { get; private set; }
    public PerformEffectGA(Effect effect)
    {
        this.Effect = effect;
    }
}
