using UnityEngine;

public class DrawCardsEffect : Effect
{
    private readonly DeckSystem deckSystem;
    [SerializeField] private int drawAmount;
    public override GameAction GetGameAction(PlayerController user)
    {
        DrawCardGA drawCardGA = new(user, drawAmount);
        return drawCardGA;
    }
    public override void ExecuteLogic(PlayerController user)
    {
        DrawCardGA drawCardGA = new(user, drawAmount);
        deckSystem.DrawCardLogicGA(drawCardGA);
    }
}
