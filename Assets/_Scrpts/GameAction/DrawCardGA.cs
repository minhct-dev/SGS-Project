using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DrawCardGA : GameAction
{
    public PlayerController Player;
    public int Amount;
    public List<CardInstanceData> DrawCardList = new();
    public DrawCardGA()
    {
    }
    public DrawCardGA(PlayerController player, int amount)
    {
        Player = player;
        Amount = amount;
    }
}
