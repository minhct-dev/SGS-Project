using UnityEngine;
using System;
using Unity.VisualScripting;
[Serializable]
public class DrawCardsEffect : Effect
{
    [SerializeField] private int drawAmount;
    public override GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard)
    {
        DrawCardGA drawCardGA = new(user, drawAmount);
        return drawCardGA;
    }

}
