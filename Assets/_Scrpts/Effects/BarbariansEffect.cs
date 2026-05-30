using UnityEngine;
using System;
using Mirror;
[Serializable]
public class BarbariansEffect : Effect
{
    public override GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard)
    {
        BarbariansGA barbariansGA = new BarbariansGA(user, sourceCard);
        return barbariansGA;
    }
}