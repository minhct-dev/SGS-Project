using Mirror.Examples.Basic;
using UnityEngine;

public class AskForCardGA : GameAction
{
    public PlayerController Target;
    public string CardID;
    public int Amount;
    public float TimeOut;

    public AskForCardGA(PlayerController target, string cardID, int amount, float timeOut)
    {
        this.Target = target;
        this.CardID = cardID;
        this.Amount = amount;
        this.TimeOut = timeOut;
    }
}
