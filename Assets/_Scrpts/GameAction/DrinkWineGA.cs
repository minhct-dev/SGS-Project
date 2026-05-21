using UnityEngine;

public class DrinkWineGA : GameAction
{
    public PlayerController User { get; private set; }
    public CardInstanceData WineCard { get; private set; }
    public DrinkWineGA(PlayerController User, CardInstanceData wineCard)
    {
        this.User = User;
        this.WineCard = wineCard;
    }

}