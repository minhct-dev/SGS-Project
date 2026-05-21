using UnityEngine;
using System;
using Mirror;
[Serializable]
public class HailOfArrowsEffect : Effect
{
    public override GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard)
    {
        HailOfArrowsGA hailOfArrowsGA = new HailOfArrowsGA(user, sourceCard);
        return hailOfArrowsGA;
    }
}