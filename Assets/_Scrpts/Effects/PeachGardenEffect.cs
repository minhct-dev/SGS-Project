using UnityEngine;
using System;
using Mirror;
[Serializable]
public class PeachGardenEffect : Effect
{
    [SerializeField] private int amount = 1;
    public override GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard)
    {
        PeachGardenGA peachGardenGA = new PeachGardenGA(user, sourceCard, amount);
        return peachGardenGA;
    }
}