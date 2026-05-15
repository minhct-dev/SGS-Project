using UnityEngine;

public class SlashGA : GameAction
{
    public PlayerController Source { get; private set; }
    public PlayerController Reciever { get; private set; }
    public int Amount { get; private set; }
    public CardInstanceData SourceCard { get; private set; }

    public bool isEvaded { get; set; } = false;

    public SlashGA(PlayerController source, PlayerController reciever, int amount, CardInstanceData sourceCard)
    {
        this.Source = source;
        this.Reciever = reciever;
        this.Amount = amount;
        this.SourceCard = sourceCard;
    }

}