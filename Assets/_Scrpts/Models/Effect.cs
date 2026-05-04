using UnityEditor.ShaderGraph.Internal;

[System.Serializable]
public abstract class Effect
{
    public abstract GameAction GetGameAction(PlayerController user);
    public abstract void ExecuteLogic(PlayerController user);
}
