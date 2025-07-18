using UnityEngine;

public class DrawCardGA : GameAction
{
    public PlayerController Player;
    public int Amount { get; private set; }
    public DrawCardGA(PlayerController player, int amount)
    {
        Player = player;
        Amount = amount;
    }
}
