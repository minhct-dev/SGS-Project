using UnityEngine;

public class DealDamageGA : GameAction
{
    public PlayerController dealer;
    public PlayerController reciever;
    public int amount;

    public DealDamageGA(PlayerController dealer, PlayerController reciever, int amount)
    {
        this.dealer = dealer;
        this.reciever = reciever;
        this.amount = amount;
    }

}