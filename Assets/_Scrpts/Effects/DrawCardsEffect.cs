using UnityEngine;

public class DrawCardsEffect : Effect
{
    [SerializeField] private int drawAmount;
    public override GameAction GetGameAction()
    {
        DrawCardGA drawCardGA = new(drawAmount);
        return drawCardGA;
    }
}
