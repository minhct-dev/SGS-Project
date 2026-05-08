using UnityEngine;

public class DealDamageGA : GameAction
{
    public PlayerController Source { get; private set; }
    public PlayerController Reciever { get; private set; }
    public int Amount { get; private set; }
    public CardInstanceData SourceCard { get; private set; }

    public bool isEvaded { get; set; } = false;

    public DealDamageGA(PlayerController source, PlayerController reciever, int amount, CardInstanceData sourceCard)
    {
        this.Source = source;
        this.Reciever = reciever;
        this.Amount = amount;
        this.SourceCard = sourceCard;
    }

}