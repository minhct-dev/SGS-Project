using UnityEngine;

public class BarbariansGA : GameAction
{
    public PlayerController User { get; private set; }
    public CardInstanceData BarbariansCard { get; private set; }

    public BarbariansGA(PlayerController user, CardInstanceData barbariansCard)
    {
        this.User = user;
        this.BarbariansCard = barbariansCard;
    }
}
