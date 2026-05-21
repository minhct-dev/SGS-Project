using UnityEngine;

public class HailOfArrowsGA : GameAction
{
    public PlayerController User { get; private set; }
    public CardInstanceData HailOfArrowsCard { get; private set; }

    public HailOfArrowsGA(PlayerController user, CardInstanceData hailOfArrowsCard)
    {
        this.User = user;
        this.HailOfArrowsCard = hailOfArrowsCard;
    }
}
