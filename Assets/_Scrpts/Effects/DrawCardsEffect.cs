using UnityEngine;

public class DrawCardsEffect : Effect
{
    [SerializeField] private int drawAmount;
    public override GameAction GetGameAction(PlayerController user)
    {
        DrawCardGA drawCardGA = new(user, drawAmount);
        return drawCardGA;
    }

}
