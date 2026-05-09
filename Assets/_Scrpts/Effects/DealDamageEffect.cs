using UnityEngine;
using System;
using Mirror;
[Serializable]
public class DealDamageEffect : Effect
{
    [SerializeField] private int amount = 1;
    public override GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard)
    {
        if (targetIds == null || targetIds.Length == 0) return null;
        uint targetId = targetIds[0];
        if (NetworkServer.spawned.TryGetValue(targetId, out NetworkIdentity identity))
        {
            PlayerController target = identity.GetComponent<PlayerController>();
            if (target != null) return new DealDamageGA(user, target, amount, sourceCard);
        }
        return null;
    }
}
