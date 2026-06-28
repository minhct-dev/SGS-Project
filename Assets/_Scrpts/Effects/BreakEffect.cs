public class BreakEffect : Effect
{

    public override GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard)
    {
        BreakGA breakGA = new BreakGA(user, user, sourceCard);
        return breakGA;
    }
}