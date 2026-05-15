using UnityEngine;

public class PeachEffect : Effect
{
    [SerializeField] private int amount = 1;
    public override GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard)
    {
        PeachGA peachGA = new PeachGA(user, user, sourceCard, amount);
        return peachGA;
    }
}