using UnityEngine;

public class TurnGA : GameAction
{
    public PlayerController player;
    public TurnGA(PlayerController player)
    {
        this.player = player;
    }
}