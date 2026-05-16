using UnityEngine;
using System;
using Mirror;
[Serializable]
public class WineEffect : Effect
{
    public override GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard)
    {
        DrinkWineGA drinkWineGA = new DrinkWineGA(user, sourceCard);
        return drinkWineGA;
    }
    public override bool IsPlayable(PlayerController player, TurnPhase currentPhase)
    {
        // 1. Luật cứu mạng: Nếu đang bị chém sắp chết -> Luôn được uống!
        if (player.isAnsweringDodge /* hoặc isDying */)
        {
            return true;
        }

        // 2. Luật Play Phase: Chỉ được uống nếu chưa uống
        if (currentPhase == TurnPhase.Play)
        {
            return !player.HasUseWineCard;
        }

        return false;
    }
}

