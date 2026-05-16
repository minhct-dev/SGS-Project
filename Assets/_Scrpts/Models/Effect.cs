using UnityEditor.ShaderGraph.Internal;

[System.Serializable]
public abstract class Effect
{
    public abstract GameAction GetGameAction(PlayerController user, uint[] targetIds, CardInstanceData sourceCard);
    public virtual bool IsPlayable(PlayerController player, TurnPhase currentPhase)
    {
        return currentPhase == TurnPhase.Play;
    }
}
