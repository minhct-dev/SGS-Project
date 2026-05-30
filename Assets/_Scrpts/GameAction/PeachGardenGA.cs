public class PeachGardenGA : GameAction
{
    public PlayerController User { get; private set; }
    public CardInstanceData PeachGardenCard { get; private set; }
    public int HealAmount { get; private set; }
    public PeachGardenGA(PlayerController user, CardInstanceData peachGardenCard, int healAmount)
    {
        this.User = user;
        this.PeachGardenCard = peachGardenCard;
        this.HealAmount = healAmount;
    }
}